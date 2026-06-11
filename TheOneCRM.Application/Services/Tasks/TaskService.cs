using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.INotifications;
using TheOneCRM.Application.Interfaces.ITasks;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.Dashboard;
using TheOneCRM.Domain.Models.DTOs.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.ProjectSpec;
using TheOneCRM.Infrastructure.Specsification.TaskSpec;
using TaskEntity = TheOneCRM.Domain.Models.Entities.Tasks;
using ProjectEntity = TheOneCRM.Domain.Models.Entities.Projects;

namespace TheOneCRM.Application.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<int> CreateTaskAsync(CreateTaskDto dto, string managerId)
        {
            if (dto.AssignedToIds == null || dto.AssignedToIds.Count == 0)
                throw new InvalidOperationException("At least one developer must be assigned to the task");

            // 1) المشروع موجود؟
            var project = await _unitOfWork.Repository<ProjectEntity>().GetByIdAsync(dto.ProjectId);
            if (project is null)
                throw new KeyNotFoundException($"Project {dto.ProjectId} not found");

            // 2) تأكد إن كل المطورين موجودين و Developer
            foreach (var devId in dto.AssignedToIds)
                await EnsureDeveloperAsync(devId);

            var task = _mapper.Map<TaskEntity>(dto);
            task.CreatedById = managerId;
            task.AssignedToId = dto.AssignedToIds[0]; // الأول كـ primary
            SyncCompletion(task);

            await _unitOfWork.Repository<TaskEntity>().AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            // أضف كل المطورين في جدول TaskAssignees
            foreach (var devId in dto.AssignedToIds)
            {
                await _unitOfWork.Repository<TaskAssignee>().AddAsync(new TaskAssignee
                {
                    TaskId = task.Id,
                    UserId = devId
                });

                await _notificationService.CreateAsync(new CreateNotificationDto
                {
                    UserId = devId,
                    Title = "مهمة جديدة",
                    Message = $"تم تعيين مهمة جديدة لك: '{task.Title}'",
                    Type = NotificationType.TaskAssigned,
                    RelatedEntityType = "Task",
                    RelatedEntityId = task.Id
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return task.Id;
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(int id)
        {
            var spec = new TaskByIdSpec(id);
            var task = await _unitOfWork.Repository<TaskEntity>()
                .GetQueryableWithSpec(spec)
                .ProjectTo<TaskResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (task is null)
                throw new KeyNotFoundException($"Task {id} not found");

            return task;
        }

        public async Task<Pagination<TaskResponseDto>> GetTasksAsync(TaskParams p)
        {
            var listSpec = new TasksListSpec(p);
            var countSpec = new TasksCountSpec(p);

            var items = await _unitOfWork.Repository<TaskEntity>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<TaskResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<TaskEntity>().CountAsync(countSpec);

            return new Pagination<TaskResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task UpdateTaskAsync(int id, UpdateTaskDto dto)
        {
            if (dto.AssignedToIds == null || dto.AssignedToIds.Count == 0)
                throw new InvalidOperationException("At least one developer must be assigned to the task");

            var task = await _unitOfWork.Repository<TaskEntity>()
                .GetEntityWithSpec(new TaskByIdSpec(id));
            if (task is null)
                throw new KeyNotFoundException($"Task {id} not found");

            foreach (var devId in dto.AssignedToIds)
                await EnsureDeveloperAsync(devId);

            // امسح القديم وضيف الجديد
            var oldAssignees = task.Assignees.ToList();
            foreach (var old in oldAssignees)
                _unitOfWork.Repository<TaskAssignee>().Delete(old);

            _mapper.Map(dto, task);
            task.AssignedToId = dto.AssignedToIds[0];
            SyncCompletion(task);

            _unitOfWork.Repository<TaskEntity>().Update(task);

            foreach (var devId in dto.AssignedToIds)
                await _unitOfWork.Repository<TaskAssignee>().AddAsync(new TaskAssignee
                {
                    TaskId = task.Id,
                    UserId = devId
                });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _unitOfWork.Repository<TaskEntity>().GetByIdAsync(id);
            if (task is null)
                throw new KeyNotFoundException($"Task {id} not found");

            _unitOfWork.Repository<TaskEntity>().Delete(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<TaskResponseDto>> GetMyTasksAsync(TaskParams p, string developerId)
        {
            var listSpec = new TasksListSpec(p, developerId);
            var countSpec = new TasksCountSpec(p, developerId);

            var items = await _unitOfWork.Repository<TaskEntity>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<TaskResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<TaskEntity>().CountAsync(countSpec);

            return new Pagination<TaskResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task<TaskResponseDto> GetMyTaskByIdAsync(int id, string developerId)
        {
            var spec = new TaskByIdForDeveloperSpec(id, developerId);
            var task = await _unitOfWork.Repository<TaskEntity>()
                .GetQueryableWithSpec(spec)
                .ProjectTo<TaskResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (task is null)
                throw new KeyNotFoundException($"Task {id} not found or not assigned to you");

            return task;
        }

        public async Task UpdateMyTaskStatusAsync(int id, UpdateTaskStatusDto dto, string developerId)
        {
            var task = await _unitOfWork.Repository<TaskEntity>()
                .GetEntityWithSpec(new TaskByIdSpec(id));
            if (task is null)
                throw new KeyNotFoundException($"Task {id} not found");

            // المطوّر يقدر يعدّل حالة مهامه هو بس
            var isAssigned = task.Assignees.Any(a => a.UserId == developerId)
                             || task.AssignedToId == developerId;
            if (!isAssigned)
                throw new UnauthorizedAccessException("You can only update your own tasks");

            // حدّث حالة المطور في TaskAssignee بس (مش الـ task الكلي)
            var assignee = task.Assignees.FirstOrDefault(a => a.UserId == developerId);
            if (assignee != null)
            {
                var wasCompleted = assignee.Status == StatusOfTask.Completed;
                assignee.Status = dto.Status;
                if (dto.ActualHours.HasValue)
                    assignee.ActualHours = dto.ActualHours;
                if (dto.Status == StatusOfTask.Completed && assignee.CompletedAt == null)
                    assignee.CompletedAt = DateTime.UtcNow;
                else if (dto.Status != StatusOfTask.Completed)
                    assignee.CompletedAt = null;

                _unitOfWork.Repository<TaskAssignee>().Update(assignee);

                // لو كل الأعضاء خلصوا → حدّث الـ task الأساسي كمان
                if (task.Assignees.All(a => a.Status == StatusOfTask.Completed))
                {
                    task.Status = StatusOfTask.Completed;
                    task.CompletedAt ??= DateTime.UtcNow;
                    _unitOfWork.Repository<TaskEntity>().Update(task);
                }

                await _unitOfWork.SaveChangesAsync();

                if (dto.Status == StatusOfTask.Completed && !wasCompleted)
                {
                    var developer = await _userManager.FindByIdAsync(developerId);
                    var developerName = developer?.FullName ?? "مطور";

                    var admins = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);
                    foreach (var admin in admins.Where(a => a.IsActive != false))
                    {
                        await _notificationService.CreateAsync(new CreateNotificationDto
                        {
                            UserId = admin.Id,
                            Title = "مهمة مكتملة",
                            Message = $"أكمل {developerName} المهمة: '{task.Title}'",
                            Type = NotificationType.TaskAssigned,
                            RelatedEntityType = "Task",
                            RelatedEntityId = task.Id
                        });
                    }
                }
            }
        }

        public async Task<DashboardStatsDto> GetDeveloperStatisticsAsync(string developerId)
        {
            var tasks = await _unitOfWork.Repository<TaskEntity>()
                .ListAsync(new TasksByDeveloperSpec(developerId));

            var projects = await _unitOfWork.Repository<ProjectEntity>()
                .ListAsync(new ProjectsByEngineerAllSpec(developerId));

            return BuildStats(tasks, projects);
        }

        public async Task<DashboardStatsDto> GetAdminStatisticsAsync()
        {
            var tasks = await _unitOfWork.Repository<TaskEntity>().ListAllAsync();
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAllAsync();

            return BuildStats(tasks, projects);
        }

        // ملاحظة: وقت إكمال المهمة بياخد CompletedAt الحقيقي (مع fallback لـ UpdatedAt/CreatedAt
        // للبيانات القديمة). تجميع المشاريع أسبوعيًا بيتم حسب CreatedAt.
        private static DashboardStatsDto BuildStats(
            IReadOnlyList<TaskEntity> tasks,
            IReadOnlyList<ProjectEntity> projects)
        {
            var today = DateTime.UtcNow.Date;
            var weekStart = StartOfWeek(today);
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var quarterStart = StartOfQuarter(today);

            var dueToday = tasks
                .Where(t => t.DueDate.Date == today && t.Status != StatusOfTask.Completed)
                .ToList();

            var stats = new DashboardStatsDto
            {
                TotalTasks         = tasks.Count,
                TodoTasks          = tasks.Count(t => t.Status == StatusOfTask.ToDo),
                InProgressTasks    = tasks.Count(t => t.Status == StatusOfTask.InProgress),
                ReviewTasks        = tasks.Count(t => t.Status == StatusOfTask.Review),
                CompletedTasks     = tasks.Count(t => t.Status == StatusOfTask.Completed),
                OverdueTasks       = tasks.Count(t => t.DueDate.Date < today && t.Status != StatusOfTask.Completed),

                TasksDueToday = dueToday.Count,
                HighPriorityTasksDueToday = dueToday.Count(t => t.Priority == PriorityStatus.High),

                OverdueProjects = projects.Count(p =>
                    p.End.Date < today &&
                    p.Status != StatusOfProject.Completed &&
                    p.Status != StatusOfProject.Cancelled),

                CompletedProjectsThisQuarter = projects.Count(p =>
                    p.Status == StatusOfProject.Completed &&
                    p.End.Date >= quarterStart && p.End.Date <= today),

                ActiveProjects = projects.Count(p => p.Status == StatusOfProject.InProgress),

                ActiveProjectsAddedThisMonth = projects.Count(p =>
                    p.Status == StatusOfProject.InProgress && p.CreatedAt >= monthStart),
            };

            // إنتاجية الأسبوع (Mon-Sat): عدد المهام المكتملة في كل يوم
            string[] dayLabels = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < dayLabels.Length; i++)
            {
                var day = weekStart.AddDays(i);
                var count = tasks.Count(t =>
                    t.Status == StatusOfTask.Completed &&
                    (t.CompletedAt ?? t.UpdatedAt ?? t.CreatedAt).Date == day);

                stats.WeeklyProductivity.Add(new DailyProductivityDto
                {
                    Day = dayLabels[i],
                    CompletedTasks = count
                });
            }

            // تقدّم المشروعات (آخر 4 أسابيع): مكتمل vs قيد التنفيذ حسب أسبوع الإنشاء
            for (int w = 0; w < 4; w++)
            {
                var wkStart = weekStart.AddDays(-7 * (3 - w));
                var wkEnd = wkStart.AddDays(7);

                var inWeek = projects
                    .Where(p => p.CreatedAt >= wkStart && p.CreatedAt < wkEnd)
                    .ToList();

                stats.ProjectsProgress.Add(new WeeklyProgressDto
                {
                    Week = $"Week {w + 1}",
                    Completed = inWeek.Count(p => p.Status == StatusOfProject.Completed),
                    InProgress = inWeek.Count(p => p.Status == StatusOfProject.InProgress)
                });
            }

            return stats;
        }

        public async Task<ControlPanelDto> GetDeveloperControlPanelAsync(string developerId)
        {
            var tasks = await _unitOfWork.Repository<TaskEntity>()
                .ListAsync(new TasksByDeveloperSpec(developerId));

            var projects = await _unitOfWork.Repository<ProjectEntity>()
                .ListAsync(new ProjectsByEngineerAllSpec(developerId));

            return BuildControlPanel(tasks, projects);
        }

        public async Task<ControlPanelDto> GetAdminControlPanelAsync()
        {
            var tasks = await _unitOfWork.Repository<TaskEntity>().ListAllAsync();
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAllAsync();

            return BuildControlPanel(tasks, projects);
        }

        // ملاحظة: "أداء السبرنت" تقريبي — مفيش كيان Sprint، فبنعتبر كل أسبوع من آخر 4 أسابيع
        // sprint: المخطط = المهام اللي موعدها النهائي في الأسبوع ده، المنجز = المهام المكتملة فيه.
        private static ControlPanelDto BuildControlPanel(
            IReadOnlyList<TaskEntity> tasks,
            IReadOnlyList<ProjectEntity> projects)
        {
            var today = DateTime.UtcNow.Date;
            var weekStart = StartOfWeek(today);

            var projectNames = projects
                .GroupBy(p => p.Id)
                .ToDictionary(g => g.Key, g => g.First().Title);

            var dueToday = tasks
                .Where(t => t.DueDate.Date == today && t.Status != StatusOfTask.Completed)
                .OrderBy(t => t.DueDate)
                .ToList();

            var panel = new ControlPanelDto
            {
                DeadlinesToday = dueToday.Count,
                CompletedTasks = tasks.Count(t => t.Status == StatusOfTask.Completed),
                AssignedTasks = tasks.Count(t => t.Status != StatusOfTask.Completed),
                ActiveProjects = projects.Count(p => p.Status == StatusOfProject.InProgress),
            };

            panel.TodayDeadlines = dueToday
                .Select(t => new DeadlineItemDto
                {
                    TaskId = t.Id,
                    Title = t.Title,
                    ProjectName = projectNames.TryGetValue(t.ProjectId, out var name) ? name : null,
                    DueDate = t.DueDate,
                    Priority = t.Priority
                })
                .ToList();

            // أداء السبرنت (آخر 4 أسابيع)
            for (int w = 0; w < 4; w++)
            {
                var wkStart = weekStart.AddDays(-7 * (3 - w));
                var wkEnd = wkStart.AddDays(7);

                panel.SprintPerformance.Add(new SprintPerformanceDto
                {
                    Sprint = $"Sprint {w + 1}",
                    Planned = tasks.Count(t => t.DueDate >= wkStart && t.DueDate < wkEnd),
                    Completed = tasks.Count(t =>
                        t.Status == StatusOfTask.Completed &&
                        (t.CompletedAt ?? t.UpdatedAt ?? t.CreatedAt) >= wkStart &&
                        (t.CompletedAt ?? t.UpdatedAt ?? t.CreatedAt) < wkEnd)
                });
            }

            return panel;
        }

        private static DateTime StartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.Date.AddDays(-diff);
        }

        private static DateTime StartOfQuarter(DateTime date)
        {
            int quarterIndex = (date.Month - 1) / 3;
            return new DateTime(date.Year, quarterIndex * 3 + 1, 1);
        }

        // يظبط CompletedAt حسب الحالة: يتسجّل عند الإكمال ويتمسح لو اترجعت المهمة
        private static void SyncCompletion(TaskEntity task)
        {
            if (task.Status == StatusOfTask.Completed)
            {
                task.CompletedAt ??= DateTime.UtcNow;
            }
            else
            {
                task.CompletedAt = null;
            }
        }

        private async Task EnsureDeveloperAsync(string developerId)
        {
            if (string.IsNullOrWhiteSpace(developerId))
                throw new InvalidOperationException("A developer must be assigned to the task");

            var user = await _userManager.FindByIdAsync(developerId);
            if (user is null)
                throw new KeyNotFoundException($"Developer {developerId} not found");

            if (!await _userManager.IsInRoleAsync(user, UserRoles.Developer))
                throw new InvalidOperationException($"User {developerId} is not a Developer");
        }
    }
}
