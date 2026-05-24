using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TheOneCRM.Application.Interfaces.IResources;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Resources;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification.TaskSpec;
using TaskEntity = TheOneCRM.Domain.Models.Entities.Tasks;

namespace TheOneCRM.Application.Services.Resources
{
    public class ResourceManagementService : IResourceManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        // السعة الأسبوعية لكل مطوّر (ساعات) وعتبات التصنيف — قابلة للتعديل
        private const int CapacityHours = 40;
        private const int OverloadedThreshold = 85; // محمّل: حمل العمل >= 85%
        private const int AvailableThreshold = 60;  // متاح: حمل العمل < 60%

        public ResourceManagementService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<WorkloadOverviewDto> GetTeamWorkloadOverviewAsync(WorkloadParams p)
        {
            var developers = await _userManager.GetUsersInRoleAsync(UserRoles.Developer);

            var tasks = await _unitOfWork.Repository<TaskEntity>()
                .ListAsync(new WorkloadTasksSpec(p.ProjectId, p.Priority));

            var tasksByDeveloper = tasks
                .GroupBy(t => t.AssignedToId!)
                .ToDictionary(g => g.Key, g => g.ToList());

            var items = developers.Select(dev =>
            {
                var devTasks = tasksByDeveloper.TryGetValue(dev.Id, out var list)
                    ? list
                    : new List<TaskEntity>();

                var usedHours = devTasks.Sum(t => t.EstimatedHours ?? 0);
                var workload = CapacityHours == 0
                    ? 0
                    : (int)Math.Round(usedHours * 100.0 / CapacityHours);

                return new DeveloperWorkloadDto
                {
                    DeveloperId = dev.Id,
                    FullName = dev.FullName,
                    Specialty = dev.Specialty,
                    TasksCount = devTasks.Count,
                    UsedHours = usedHours,
                    AvailableHours = Math.Max(0, CapacityHours - usedHours),
                    CapacityHours = CapacityHours,
                    WorkloadPercent = workload
                };
            }).ToList();

            items = ApplySort(items, p.Sort);

            return new WorkloadOverviewDto
            {
                TotalDevelopers = developers.Count,
                AvailableDevelopers = items.Count(i => i.WorkloadPercent < AvailableThreshold),
                OverloadedDevelopers = items.Count(i => i.WorkloadPercent >= OverloadedThreshold),
                AverageWorkload = items.Count == 0
                    ? 0
                    : (int)Math.Round(items.Average(i => i.WorkloadPercent)),
                Developers = items
            };
        }

        public async Task<List<WorkloadDistributionItemDto>> GetWorkloadDistributionAsync()
        {
            var overview = await GetTeamWorkloadOverviewAsync(new WorkloadParams());

            return overview.Developers
                .OrderByDescending(d => d.WorkloadPercent)
                .Select(d => new WorkloadDistributionItemDto
                {
                    DeveloperId = d.DeveloperId,
                    FullName = d.FullName,
                    TasksCount = d.TasksCount,
                    WorkloadPercent = d.WorkloadPercent
                })
                .ToList();
        }

        private static List<DeveloperWorkloadDto> ApplySort(List<DeveloperWorkloadDto> items, WorkloadSort sort) => sort switch
        {
            WorkloadSort.WorkloadAsc => items.OrderBy(i => i.WorkloadPercent).ToList(),
            WorkloadSort.TasksDesc => items.OrderByDescending(i => i.TasksCount).ToList(),
            WorkloadSort.NameAsc => items.OrderBy(i => i.FullName).ToList(),
            _ => items.OrderByDescending(i => i.WorkloadPercent).ToList(),
        };
    }
}
