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
using TheOneCRM.Infrastructure.Specsification.TaskSpec;
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
            var allTasks = await _unitOfWork.Repository<TaskEntity>().ListAsync(new AllTasksWithAssigneesSpec());
            var tasks = ApplyFilters(allTasks, p, applyPeriod: true);
            var developers = await GetDevelopersAsync(p.DeveloperId);
            return BuildSummary(tasks, developers, p.DeveloperId);
        }

        public async Task<List<DeveloperStatItemDto>> GetDeveloperStatsAsync(DeveloperAnalyticsParams p)
        {
            var allTasks = await _unitOfWork.Repository<TaskEntity>().ListAsync(new AllTasksWithAssigneesSpec());
            var tasks = ApplyFilters(allTasks, p, applyPeriod: true);
            var developers = await GetDevelopersAsync(p.DeveloperId);
            return BuildDeveloperStats(developers, tasks).OrderByDescending(d => d.CompletedTasks).ToList();
        }

        public async Task<AnalyticsChartsDto> GetChartsAsync(DeveloperAnalyticsParams p)
        {
            var allTasks = await _unitOfWork.Repository<TaskEntity>().ListAsync(new AllTasksWithAssigneesSpec());
            var tasks = ApplyFilters(allTasks, p, applyPeriod: false);
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAsync(new ProjectsForDropdownSpec(p.DeveloperId));
            return BuildCharts(tasks, projects, p.DeveloperId);
        }

        public async Task<BugAnalyticsDto> GetBugAnalyticsAsync(DeveloperAnalyticsParams p)
        {
            var allTasks = await _unitOfWork.Repository<TaskEntity>().ListAsync(new AllTasksWithAssigneesSpec());
            var tasks = ApplyFilters(allTasks, p, applyPeriod: false);
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAllAsync();
            return BuildBugAnalytics(tasks, projects);
        }

        public async Task<FullDeveloperAnalyticsDto> GetFullAnalyticsAsync(DeveloperAnalyticsParams p)
        {
            // نحمّل الـ tasks مرة واحدة ونستخدمها في الكل
            var allTasks = await _unitOfWork.Repository<TaskEntity>().ListAsync(new AllTasksWithAssigneesSpec());
            var developers = await GetDevelopersAsync(p.DeveloperId);
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAsync(new ProjectsForDropdownSpec(p.DeveloperId));

            var filteredByPeriod = ApplyFilters(allTasks, p, applyPeriod: true);
            var filteredNoPeriod = ApplyFilters(allTasks, p, applyPeriod: false);

            return new FullDeveloperAnalyticsDto
            {
                Summary = BuildSummary(filteredByPeriod, developers, p.DeveloperId),
                DeveloperStats = BuildDeveloperStats(developers, filteredByPeriod)
                    .OrderByDescending(d => d.CompletedTasks).ToList(),
                Charts = BuildCharts(filteredNoPeriod, projects, p.DeveloperId),
                BugAnalytics = BuildBugAnalytics(filteredNoPeriod, projects)
            };
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

        private static IReadOnlyList<TaskEntity> ApplyFilters(
            IReadOnlyList<TaskEntity> tasks, DeveloperAnalyticsParams p, bool applyPeriod)
        {
            IEnumerable<TaskEntity> q = tasks;

            if (!string.IsNullOrEmpty(p.DeveloperId))
                q = q.Where(t => t.AssignedToId == p.DeveloperId ||
                                 t.Assignees.Any(a => a.UserId == p.DeveloperId));

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
                    var quarter = (today.Month - 1) / 3;
                    var qStart = new DateTime(today.Year, quarter * 3 + 1, 1);
                    return (qStart, qStart.AddMonths(3));

                case AnalyticsPeriod.HalfYear:
                    var halfStart = today.Month <= 6
                        ? new DateTime(today.Year, 1, 1)
                        : new DateTime(today.Year, 7, 1);
                    return (halfStart, halfStart.AddMonths(6));

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
                // استخدم نفس GetAssigneeRecords للـ fallback التلقائي
                var devTasks = tasks
                    .Where(t => t.AssignedToId == dev.Id || t.Assignees.Any(a => a.UserId == dev.Id))
                    .ToList();

                var devAssignees = GetAssigneeRecords(devTasks, dev.Id);
                var completedAssignees = devAssignees.Where(a => a.Status == StatusOfTask.Completed).ToList();
                var activeHours = devAssignees
                    .Where(a => a.Status != StatusOfTask.Completed)
                    .Sum(a =>
                    {
                        var t = tasks.FirstOrDefault(x => x.Id == a.TaskId);
                        return t?.EstimatedHours ?? 0;
                    });

                return new DeveloperStatItemDto
                {
                    DeveloperId = dev.Id,
                    FullName = dev.FullName,
                    CompletedTasks = completedAssignees.Count,
                    AvgCompletionTimeHours = completedAssignees.Any()
                        ? Math.Round(completedAssignees.Average(a =>
                        {
                            var t = tasks.FirstOrDefault(x => x.Id == a.TaskId);
                            return (double)(a.ActualHours ?? t?.EstimatedHours ?? 0);
                        }), 1)
                        : 0,
                    ResolvedBugs = devAssignees.Count(a =>
                        a.Status == StatusOfTask.Completed &&
                        tasks.FirstOrDefault(t => t.Id == a.TaskId)?.Category == TaskCategory.Bug),
                    CurrentWorkloadPercent = Percent(activeHours, CapacityHours),
                    ProductivityPercent = Percent(completedAssignees.Count, devAssignees.Count)
                };
            }).ToList();
        }

        private static DeveloperAnalyticsSummaryDto BuildSummary(
            IReadOnlyList<TaskEntity> tasks,
            IList<AppUser> developers,
            string? developerId)
        {
            var today = DateTime.UtcNow.Date;
            var assigneeRecords = GetAssigneeRecords(tasks, developerId);
            var completedRecords = assigneeRecords.Where(a => a.Status == StatusOfTask.Completed).ToList();

            var thisMonth = completedRecords.Count(a => SameMonth(AssigneeCompletionDate(a, tasks), today));
            var lastMonthRef = today.AddMonths(-1);
            var lastMonth = completedRecords.Count(a => SameMonth(AssigneeCompletionDate(a, tasks), lastMonthRef));

            var weekStart = StartOfWeek(today);
            var prevWeekStart = weekStart.AddDays(-7);
            var completedThisWeek = completedRecords.Count(a => AssigneeCompletionDate(a, tasks) >= weekStart);
            var completedLastWeek = completedRecords.Count(a =>
                AssigneeCompletionDate(a, tasks) >= prevWeekStart && AssigneeCompletionDate(a, tasks) < weekStart);

            var devStats = BuildDeveloperStats(developers, tasks);
            var mostProductive = devStats.OrderByDescending(d => d.ProductivityPercent).FirstOrDefault();
            var fastest = devStats.Where(d => d.CompletedTasks > 0).OrderBy(d => d.AvgCompletionTimeHours).FirstOrDefault();

            var topPerformer = developers
                .Select(d => new
                {
                    d,
                    count = GetAssigneeRecords(tasks, d.Id)
                        .Count(a => a.Status == StatusOfTask.Completed &&
                                    SameMonth(AssigneeCompletionDate(a, tasks), today))
                })
                .OrderByDescending(x => x.count)
                .FirstOrDefault();

            return new DeveloperAnalyticsSummaryDto
            {
                ProductivityRate = Percent(completedRecords.Count, assigneeRecords.Count),
                ProductivityChangePercent = ChangePercent(completedThisWeek, completedLastWeek),
                ResolvedBugs = assigneeRecords.Count(a =>
                    a.Status == StatusOfTask.Completed &&
                    tasks.FirstOrDefault(t => t.Id == a.TaskId)?.Category == TaskCategory.Bug),
                OpenBugs = assigneeRecords.Count(a =>
                    a.Status != StatusOfTask.Completed &&
                    tasks.FirstOrDefault(t => t.Id == a.TaskId)?.Category == TaskCategory.Bug),
                AvgCompletionTimeHours = completedRecords.Any()
                    ? Math.Round(completedRecords.Average(a =>
                    {
                        var t = tasks.FirstOrDefault(x => x.Id == a.TaskId);
                        return (double)(a.ActualHours ?? t?.EstimatedHours ?? 0);
                    }), 1)
                    : 0,
                CompletedTasks = completedRecords.Count,
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

        private static AnalyticsChartsDto BuildCharts(
            IReadOnlyList<TaskEntity> tasks,
            IReadOnlyList<ProjectEntity> projects,
            string? developerId)
        {
            var today = DateTime.UtcNow.Date;
            var weekStart = StartOfWeek(today);
            var charts = new AnalyticsChartsDto();

            string[] dayLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            var assigneeRecords = GetAssigneeRecords(tasks, developerId);

            for (int i = 0; i < dayLabels.Length; i++)
            {
                var day = weekStart.AddDays(i);
                charts.TaskCompletionOverTime.Add(new TaskCompletionPointDto
                {
                    Day = dayLabels[i],
                    Completed = assigneeRecords.Count(a =>
                        a.Status == StatusOfTask.Completed && a.CompletedAt?.Date == day),
                    Pending = tasks.Count(t =>
                        (developerId == null ||
                         t.AssignedToId == developerId ||
                         t.Assignees.Any(a => a.UserId == developerId)) &&
                        t.Status != StatusOfTask.Completed &&
                        t.DueDate.Date == day)
                });
            }

            foreach (var project in projects)
            {
                var projectTasks = tasks.Where(t => t.ProjectId == project.Id).ToList();
                var projectAssignees = GetAssigneeRecords(projectTasks, developerId);
                var completedCount = projectAssignees.Count(a => a.Status == StatusOfTask.Completed);

                charts.ProjectsProgress.Add(new ProjectProgressItemDto
                {
                    ProjectId = project.Id,
                    ProjectName = project.Title,
                    Progress = Percent(completedCount, projectAssignees.Count > 0 ? projectAssignees.Count : projectTasks.Count)
                });
            }

            return charts;
        }

        private static BugAnalyticsDto BuildBugAnalytics(
            IReadOnlyList<TaskEntity> tasks,
            IReadOnlyList<ProjectEntity> projects)
        {
            var projectNames = projects.ToDictionary(p => p.Id, p => p.Title);
            var bugs = tasks.Where(t => t.Category == TaskCategory.Bug).ToList();
            var totalBugs = bugs.Count;
            var today = DateTime.UtcNow.Date;
            var result = new BugAnalyticsDto();

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

            for (int i = 4; i >= 0; i--)
            {
                var monthRef = today.AddMonths(-i);
                result.MonthlyOpenVsResolved.Add(new MonthlyBugDto
                {
                    Month = monthRef.ToString("MMM", CultureInfo.InvariantCulture),
                    Open = bugs.Count(b => b.Status != StatusOfTask.Completed && SameMonth(b.CreatedAt, monthRef)),
                    Resolved = bugs.Count(b => b.Status == StatusOfTask.Completed && SameMonth(CompletionDate(b), monthRef))
                });
            }

            return result;
        }

        // سجلات TaskAssignee الفعلية — مع fallback للمهام القديمة اللي ملهاش TaskAssignee
        private static List<TaskAssignee> GetAssigneeRecords(IReadOnlyList<TaskEntity> tasks, string? developerId)
        {
            var result = new List<TaskAssignee>();
            foreach (var task in tasks)
            {
                if (task.Assignees.Any())
                {
                    result.AddRange(task.Assignees
                        .Where(a => developerId == null || a.UserId == developerId));
                }
                else if (!string.IsNullOrEmpty(task.AssignedToId) &&
                         (developerId == null || task.AssignedToId == developerId))
                {
                    // مهمة قديمة بدون TaskAssignee — نعمل record وهمي من حالة المهمة
                    result.Add(new TaskAssignee
                    {
                        TaskId = task.Id,
                        UserId = task.AssignedToId,
                        Status = task.Status,
                        ActualHours = task.ActualHours,
                        CompletedAt = task.CompletedAt
                    });
                }
            }
            return result;
        }

        // تاريخ إكمال TaskAssignee: CompletedAt الحقيقي، وإلا تاريخ إكمال المهمة الرئيسية
        private static DateTime AssigneeCompletionDate(TaskAssignee a, IReadOnlyList<TaskEntity> tasks)
        {
            if (a.CompletedAt.HasValue) return a.CompletedAt.Value.Date;
            var t = tasks.FirstOrDefault(x => x.Id == a.TaskId);
            return CompletionDate(t!);
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
