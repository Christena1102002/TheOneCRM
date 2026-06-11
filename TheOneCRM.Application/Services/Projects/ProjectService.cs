using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.INotifications;
using TheOneCRM.Application.Interfaces.IProjects;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.DTOs.Projects;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.ProjectSpec;
using Project = TheOneCRM.Domain.Models.Entities.Projects;

namespace TheOneCRM.Application.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<ProjectCustomerLookupDto> GetCustomerLookupAsync(int customerId)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer {customerId} not found");

            return _mapper.Map<ProjectCustomerLookupDto>(customer);
        }

        public async Task<int> CreateProjectAsync(CreateProjectDto dto, string userId)
        {
            // 1) العميل موجود؟
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(dto.CustomerId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer {dto.CustomerId} not found");

            // 2) validation التواريخ
            if (dto.End < dto.Start)
                throw new InvalidOperationException("End date cannot be before start date");

            // 3) المهندسون مطلوبون ولازم يكونوا Developers
            var engineerIds = NormalizeEngineerIds(dto.EngineerIds);
            await ValidateEngineersAsync(engineerIds);

            var project = _mapper.Map<Project>(dto);
            project.CreatedById = userId;
            project.ProjectEngineers = engineerIds
                .Select(id => new ProjectEngineer { EngineerId = id })
                .ToList();

            await _unitOfWork.Repository<Project>().AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            // إشعار لكل مهندس معيّن على المشروع
            foreach (var engId in engineerIds)
            {
                await _notificationService.CreateAsync(new CreateNotificationDto
                {
                    UserId = engId,
                    Title = "مشروع جديد",
                    Message = $"تم تعيينك على مشروع جديد: '{project.Title}'",
                    Type = NotificationType.General,
                    RelatedEntityType = "Project",
                    RelatedEntityId = project.Id
                });
            }

            return project.Id;
        }

        public async Task<ProjectResponseDto> GetProjectByIdAsync(int id)
        {
            var spec = new ProjectByIdSpec(id);
            var project = await _unitOfWork.Repository<Project>()
                .GetQueryableWithSpec(spec)
                .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (project is null)
                throw new KeyNotFoundException($"Project {id} not found");

            return project;
        }

        public async Task<Pagination<ProjectResponseDto>> GetProjectsAsync(ProjectParams p)
        {
            var listSpec = new ProjectsListSpec(p);
            var countSpec = new ProjectsCountSpec(p);

            var items = await _unitOfWork.Repository<Project>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<Project>().CountAsync(countSpec);

            return new Pagination<ProjectResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task UpdateProjectAsync(int id, UpdateProjectDto dto)
        {
            // 1) جلب المشروع مع المهندسين (tracked)
            var spec = new ProjectWithEngineersSpec(id);
            var project = await _unitOfWork.Repository<Project>().GetEntityWithSpec(spec);
            if (project is null)
                throw new KeyNotFoundException($"Project {id} not found");

            // 2) validation التواريخ
            if (dto.End < dto.Start)
                throw new InvalidOperationException("End date cannot be before start date");

            // 3) المهندسون
            var engineerIds = NormalizeEngineerIds(dto.EngineerIds);
            await ValidateEngineersAsync(engineerIds);

            // 4) تحديث الحقول
            _mapper.Map(dto, project);

            // 6) مزامنة المهندسين بالـ diff (نشيل اللي اتشال ونضيف الجديد بس)
            //    عشان نتجنّب تعارض delete+insert لنفس الـ composite key في EF
            foreach (var removed in project.ProjectEngineers
                         .Where(pe => !engineerIds.Contains(pe.EngineerId))
                         .ToList())
            {
                project.ProjectEngineers.Remove(removed);
            }

            var currentIds = project.ProjectEngineers
                .Select(pe => pe.EngineerId)
                .ToHashSet();

            foreach (var engId in engineerIds.Where(id => !currentIds.Contains(id)))
            {
                project.ProjectEngineers.Add(new ProjectEngineer { ProjectId = project.Id, EngineerId = engId });
            }

            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            if (project is null)
                throw new KeyNotFoundException($"Project {id} not found");

            // المهندسون بيتمسحوا cascade من جدول الربط
            _unitOfWork.Repository<Project>().Delete(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Pagination<ProjectResponseDto>> GetMyProjectsAsync(ProjectParams p, string developerId)
        {
            var listSpec = new ProjectsByEngineerSpec(p, developerId);
            var countSpec = new ProjectsByEngineerCountSpec(p, developerId);

            var items = await _unitOfWork.Repository<Project>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<Project>().CountAsync(countSpec);

            return new Pagination<ProjectResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task<ProjectResponseDto> GetMyProjectByIdAsync(int id, string developerId)
        {
            var spec = new ProjectByIdForEngineerSpec(id, developerId);
            var project = await _unitOfWork.Repository<Project>()
                .GetQueryableWithSpec(spec)
                .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (project is null)
                throw new KeyNotFoundException($"Project {id} not found or not assigned to you");

            return project;
        }

        public async Task<Pagination<ProjectResponseDto>> GetActiveProjectsAsync(ProjectParams p)
        {
            var listSpec = new ActiveProjectsSpec(p);
            var countSpec = new ActiveProjectsCountSpec(p);

            var items = await _unitOfWork.Repository<Project>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<Project>().CountAsync(countSpec);

            return new Pagination<ProjectResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task<Pagination<ProjectResponseDto>> GetMyActiveProjectsAsync(ProjectParams p, string developerId)
        {
            var listSpec = new MyActiveProjectsSpec(p, developerId);
            var countSpec = new MyActiveProjectsCountSpec(p, developerId);

            var items = await _unitOfWork.Repository<Project>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<Project>().CountAsync(countSpec);

            return new Pagination<ProjectResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        private static List<string> NormalizeEngineerIds(List<string>? engineerIds)
        {
            var ids = engineerIds?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList() ?? new List<string>();

            if (ids.Count == 0)
                throw new InvalidOperationException("At least one engineer must be assigned to the project");

            return ids;
        }

        private async Task ValidateEngineersAsync(IEnumerable<string> engineerIds)
        {
            foreach (var engId in engineerIds)
            {
                var user = await _userManager.FindByIdAsync(engId);
                if (user is null)
                    throw new KeyNotFoundException($"Engineer {engId} not found");

                if (!await _userManager.IsInRoleAsync(user, UserRoles.Developer))
                    throw new InvalidOperationException($"User {engId} is not a Developer");
            }
        }
    }
}
