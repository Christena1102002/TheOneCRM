using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.Tasks;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // ============ Create: DTO -> Entity ============
            CreateMap<CreateTaskDto, Tasks>()
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.Project, opt => opt.Ignore())
                .ForMember(d => d.AssignedToId, opt => opt.Ignore())
                .ForMember(d => d.AssignedTo, opt => opt.Ignore())
                .ForMember(d => d.Assignees, opt => opt.Ignore())
                .ForMember(d => d.ActualHours, opt => opt.Ignore())
                .ForMember(d => d.CompletedAt, opt => opt.Ignore());

            // ============ Update: DTO -> Entity ============
            CreateMap<UpdateTaskDto, Tasks>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ProjectId, opt => opt.Ignore())
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.Project, opt => opt.Ignore())
                .ForMember(d => d.AssignedToId, opt => opt.Ignore())
                .ForMember(d => d.AssignedTo, opt => opt.Ignore())
                .ForMember(d => d.Assignees, opt => opt.Ignore())
                .ForMember(d => d.ActualHours, opt => opt.Ignore())
                .ForMember(d => d.CompletedAt, opt => opt.Ignore());

            // ============ TaskAssignee -> DTO ============
            CreateMap<TaskAssignee, TaskAssigneeDto>()
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => s.User != null ? s.User.FullName : null));

            // ============ Entity -> Response ============
            CreateMap<Tasks, TaskResponseDto>()
                .ForMember(d => d.ProjectName,
                    opt => opt.MapFrom(s => s.Project != null ? s.Project.Title : null))
                .ForMember(d => d.CreatedByName,
                    opt => opt.MapFrom(s => s.CreatedBy != null ? s.CreatedBy.FullName : null))
                .ForMember(d => d.Assignees,
                    opt => opt.MapFrom(s => s.Assignees))
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.PriorityName, opt => opt.MapFrom(s => s.Priority.ToString()))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.ToString()));
        }
    }
}
