using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TheOneCRM.Application.Interfaces.IAnalytics;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Analytics;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.ProjectSpec;
using TaskEntity = TheOneCRM.Domain.Models.Entities.Tasks;
using ProjectEntity = TheOneCRM.Domain.Models.Entities.Projects;

namespace TheOneCRM.Application.Services.Analytics
{
    // ملاحظات:
    // - "الأخطاء" = مهام فئتها Bug. المحلولة = Completed، المفتوحة = غير Completed.
    // - "وقت الإنجاز" = ActualHours الفعلية، وإلا EstimatedHours لو المطوّر مسجّلش.
    // - "الإنتاجية" = المهام المكتملة ÷ إجمالي المهام × 100.
    // - وقت إكمال المهمة = CompletedAt الحقيقي (مع fallback للبيانات القديمة).
    public class DeveloperAnalyticsService : IDeveloperAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        private const int CapacityHours = 40;

        public DeveloperAnalyticsService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<DeveloperAnalyticsSummaryDto> GetSummaryAsync(DeveloperAnalyticsParams p)
        {
            var tasks = ApplyFilters(await _unitOfWork.Repository<TaskEntity>().ListAllAsync(), p, applyPeriod: true);
            var developers = await GetDevelopersAsync(p.DeveloperId);

            var today = DateTime.UtcNow.Date;
            var completed = tasks.Where(t => t.Status == StatusOfTask.Completed).ToList();
            var totalTasks = tasks.Count;

            // التغيّر شهريًا في المهام المكتملة
            var thisMonth = tasks.Count(t =>
                t.Status == StatusOfTask.Completed && SameMonth(CompletionDate(t), today));
            var lastMonthRef = today.AddMonths(-1);
            var lastMonth = tasks.Count(t =>
                t.Status == StatusOfTask.Completed && SameMonth(CompletionDate(t), lastMonthRef));

            // التغيّر أسبوعيًا (مؤشّر للإنتاجية)
            var weekStart = StartOfWeek(today);
            var prevWeekStart = weekStart.AddDays(-7);
            var completedThisWeek = completed.Count(t => CompletionDate(t) >= weekStart);
            var completedLastWeek = completed.Count(t =>
                CompletionDate(t) >= prevWeekStart && CompletionDate(t) < weekStart);

            var devStats = BuildDeveloperStats(developers, tasks);

            var mostProductive = devStats
                .OrderByDescending(d => d.ProductivityPercent)
                .FirstOrDefault();

            var fastest = devStats
                .Where(d => d.CompletedTasks > 0)
                .OrderBy(d => d.AvgCompletionTimeHours)
                .FirstOrDefault();

            var topPerformer = developers
                .Select(d => new
                {
                    d,
                    count = tasks.Count(t =>
                        t.AssignedToId == d.Id &&
                        t.Status == StatusOfTask.Completed &&
                        SameMonth(CompletionDate(t), today))
                })
                .OrderByDescending(x => x.count)
                .FirstOrDefault();

            return new DeveloperAnalyticsSummaryDto
            {
                ProductivityRate = Percent(completed.Count, totalTasks),
                ProductivityChangePercent = ChangePercent(completedThisWeek, completedLastWeek),

                ResolvedBugs = tasks.Count(t => t.Category == TaskCategory.Bug && t.Status == StatusOfTask.Completed),
                OpenBugs = tasks.Count(t => t.Category == TaskCategory.Bug && t.Status != StatusOfTask.Completed),

                AvgCompletionTimeHours = completed.Any()
                    ? Math.Round(completed.Average(t => t.ActualHours ?? t.EstimatedHours ?? 0), 1)
                    : 0,

                CompletedTasks = completed.Count,
                CompletedTasksChangePercent = ChangePercent(thisMonth, lastMonth),

                MostProductive = mostProductive is null ? null : new TopDeveloperDto
                {
                    DeveloperId = mostProductive.DeveloperId,
                    FullName = mostProductive.FullName,
                    Value = mostProductive.ProductivityPercent,
                    Label = "معدل الإنتاجية"
                },
                Fastest = fastest is null ? null : new TopDeveloperDto
                {
                    DeveloperId = fastest.DeveloperId,
                    FullName = fastest.FullName,
                    Value = fastest.AvgCompletionTimeHours,
                    Label = "ساعات متوسط وقت الإنجاز"
                },
                TopPerformer = topPerformer is null ? null : new TopDeveloperDto
                {
                    DeveloperId = topPerformer.d.Id,
                    FullName = topPerformer.d.FullName,
                    Value = topPerformer.count,
                    Label = "مهمة مكتملة هذا الشهر"
                }
            };
        }

        public async Task<List<DeveloperStatItemDto>> GetDeveloperStatsAsync(DeveloperAnalyticsParams p)
        {
            var tasks = ApplyFilters(await _unitOfWork.Repository<TaskEntity>().ListAllAsync(), p, applyPeriod: true);
            var developers = await GetDevelopersAsync(p.DeveloperId);

            return BuildDeveloperStats(developers, tasks)
                .OrderByDescending(d => d.CompletedTasks)
                .ToList();
        }

        public async Task<AnalyticsChartsDto> GetChartsAsync(DeveloperAnalyticsParams p)
        {
            // الشارت ليه محور زمني خاص بيه (أيام الأسبوع/شهور)، فمنطبّقش فلتر الفترة عليه
            var tasks = ApplyFilters(await _unitOfWork.Repository<TaskEntity>().ListAllAsync(), p, applyPeriod: false);
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAllAsync();

            var today = DateTime.UtcNow.Date;
            var weekStart = StartOfWeek(today);

            var charts = new AnalyticsChartsDto();

            // إنجاز المهام عبر الزمن (الأحد .. السبت)
            string[] dayLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < dayLabels.Length; i++)
            {
                var day = weekStart.AddDays(i);
                charts.TaskCompletionOverTime.Add(new TaskCompletionPointDto
                {
                    Day = dayLabels[i],
                    Completed = tasks.Count(t =>
                        t.Status == StatusOfTask.Completed && CompletionDate(t) == day),
                    Pending = tasks.Count(t =>
                        t.Status != StatusOfTask.Completed && t.DueDate.Date == day)
                });
            }

            // تقدّم المشاريع
            foreach (var project in projects)
            {
                var projectTasks = tasks.Where(t => t.ProjectId == project.Id).ToList();
                var completedCount = projectTasks.Count(t => t.Status == StatusOfTask.Completed);

                charts.ProjectsProgress.Add(new ProjectProgressItemDto
                {
                    ProjectId = project.Id,
                    ProjectName = project.Title,
                    Progress = Percent(completedCount, projectTasks.Count)
                });
            }

            return charts;
        }

        public async Task<BugAnalyticsDto> GetBugAnalyticsAsync(DeveloperAnalyticsParams p)
        {
            // التحليل الشهري ليه محوره الزمني الخاص (آخر 5 شهور)، فمنطبّقش فلتر الفترة هنا
            var tasks = ApplyFilters(await _unitOfWork.Repository<TaskEntity>().ListAllAsync(), p, applyPeriod: false);
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAllAsync();

            var projectNames = projects
                .GroupBy(p => p.Id)
                .ToDictionary(g => g.Key, g => g.First().Title);

            var bugs = tasks.Where(t => t.Category == TaskCategory.Bug).ToList();
            var totalBugs = bugs.Count;

            var result = new BugAnalyticsDto();

            // توزيع الأخطاء حسب المشروع
            result.DistributionByProject = bugs
                .GroupBy(b => b.ProjectId)
                .Select(g => new BugByProjectDto
                {
                    ProjectId = g.Key,
                    ProjectName = projectNames.TryGetValue(g.Key, out var name) ? name : $"#{g.Key}",
                    Count = g.Count(),
                    Percent = Percent(g.Count(), totalBugs)
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // مفتوحة vs محلولة (آخر 5 شهور)
            var today = DateTime.UtcNow.Date;
            for (int i = 4; i >= 0; i--)
            {
                var monthRef = today.AddMonths(-i);
                result.MonthlyOpenVsResolved.Add(new MonthlyBugDto
                {
                    Month = monthRef.ToString("MMM", CultureInfo.InvariantCulture),
                    Open = bugs.Count(b =>
                        b.Status != StatusOfTask.Completed && SameMonth(b.CreatedAt, monthRef)),
                    Resolved = bugs.Count(b =>
                        b.Status == StatusOfTask.Completed && SameMonth(CompletionDate(b), monthRef))
                });
            }

            return result;
        }

        // قائمة المشاريع للـ dropdown — مشاريع المطوّر المختار (أو الكل لو developerId = null)
        public async Task<List<StatusClientDto>> GetProjectOptionsAsync(string? developerId)
        {
            var items = await _unitOfWork.Repository<ProjectEntity>().ListWithSelectAsync(
                new ProjectsForDropdownSpec(developerId),
                p => new StatusClientDto { Id = p.Id, Name = p.Title });

            return items.ToList();
        }

        // ===== Helpers =====

        // فلترة المهام بالمطوّر + المشروع + (اختياري) الفترة الزمنية
        private static IReadOnlyList<TaskEntity> ApplyFilters(
            IReadOnlyList<TaskEntity> tasks, DeveloperAnalyticsParams p, bool applyPeriod)
        {
            IEnumerable<TaskEntity> q = tasks;

            if (!string.IsNullOrEmpty(p.DeveloperId))
                q = q.Where(t => t.AssignedToId == p.DeveloperId);

            if (p.ProjectId.HasValue)
                q = q.Where(t => t.ProjectId == p.ProjectId.Value);

            if (applyPeriod)
            {
                var (start, end) = PeriodRange(p.Period, DateTime.UtcNow.Date);
                q = q.Where(t =>
                    (t.CreatedAt >= start && t.CreatedAt < end) ||
                    (t.Status == StatusOfTask.Completed &&
                     CompletionDate(t) >= start && CompletionDate(t) < end));
            }

            return q.ToList();
        }

        // حساب مدى التاريخ حسب الفترة المختارة
        private static (DateTime start, DateTime end) PeriodRange(AnalyticsPeriod period, DateTime today)
        {
            switch (period)
            {
                case AnalyticsPeriod.CurrentWeek:
                    var weekStart = StartOfWeek(today);
                    return (weekStart, weekStart.AddDays(7));

                case AnalyticsPeriod.CurrentQuarter:
                    var quarter = (today.Month - 1) / 3;          // 0..3
                    var qStart = new DateTime(today.Year, quarter * 3 + 1, 1);
                    return (qStart, qStart.AddMonths(3));

                case AnalyticsPeriod.CurrentYear:
                    var yStart = new DateTime(today.Year, 1, 1);
                    return (yStart, yStart.AddYears(1));

                case AnalyticsPeriod.CurrentMonth:
                default:
                    var mStart = new DateTime(today.Year, today.Month, 1);
                    return (mStart, mStart.AddMonths(1));
            }
        }

        // قائمة المطورين (أو مطوّر واحد لو developerId محدّد)
        private async Task<IList<AppUser>> GetDevelopersAsync(string? developerId)
        {
            var developers = await _userManager.GetUsersInRoleAsync(UserRoles.Developer);
            if (!string.IsNullOrEmpty(developerId))
                developers = developers.Where(d => d.Id == developerId).ToList();
            return developers;
        }

        private static List<DeveloperStatItemDto> BuildDeveloperStats(
            IList<AppUser> developers,
            IReadOnlyList<TaskEntity> tasks)
        {
            return developers.Select(dev =>
            {
                var devTasks = tasks.Where(t => t.AssignedToId == dev.Id).ToList();
                var completed = devTasks.Where(t => t.Status == StatusOfTask.Completed).ToList();
                var activeHours = devTasks
                    .Where(t => t.Status != StatusOfTask.Completed)
                    .Sum(t => t.EstimatedHours ?? 0);

                return new DeveloperStatItemDto
                {
                    DeveloperId = dev.Id,
                    FullName = dev.FullName,
                    CompletedTasks = completed.Count,
                    AvgCompletionTimeHours = completed.Any()
                        ? Math.Round(completed.Average(t => t.ActualHours ?? t.EstimatedHours ?? 0), 1)
                        : 0,
                    ResolvedBugs = devTasks.Count(t =>
                        t.Category == TaskCategory.Bug && t.Status == StatusOfTask.Completed),
                    CurrentWorkloadPercent = Percent(activeHours, CapacityHours),
                    ProductivityPercent = Percent(completed.Count, devTasks.Count)
                };
            }).ToList();
        }

        // تاريخ إكمال المهمة: CompletedAt الحقيقي، وإلا UpdatedAt/CreatedAt للبيانات القديمة
        private static DateTime CompletionDate(TaskEntity t) => (t.CompletedAt ?? t.UpdatedAt ?? t.CreatedAt).Date;

        private static bool SameMonth(DateTime a, DateTime b) => a.Year == b.Year && a.Month == b.Month;

        private static int Percent(int part, int total) =>
            total == 0 ? 0 : (int)Math.Round(part * 100.0 / total);

        private static int ChangePercent(int current, int previous) =>
            previous == 0 ? (current > 0 ? 100 : 0) : (int)Math.Round((current - previous) * 100.0 / previous);

        private static DateTime StartOfWeek(DateTime date)
        {
            // أسبوع يبدأ من الأحد
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.Date.AddDays(-diff);
        }
    }
}
