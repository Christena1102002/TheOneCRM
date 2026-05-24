using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.Projects;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // ============ Create: DTO -> Entity ============
            CreateMap<CreateProjectDto, Projects>()
                .ForMember(d => d.ProjectEngineers, opt => opt.Ignore())
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.Customer, opt => opt.Ignore())
                .ForMember(d => d.ProjectManager, opt => opt.Ignore());

            // ============ Update: DTO -> Entity ============
            CreateMap<UpdateProjectDto, Projects>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CustomerId, opt => opt.Ignore())
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.Customer, opt => opt.Ignore())
                .ForMember(d => d.ProjectManager, opt => opt.Ignore())
                .ForMember(d => d.ProjectEngineers, opt => opt.Ignore());

            // ============ Customer lookup ============
            CreateMap<Customer, ProjectCustomerLookupDto>();

            // ============ Engineer (join) -> Dto ============
            CreateMap<ProjectEngineer, ProjectEngineerDto>()
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => s.Engineer != null ? s.Engineer.FullName : null));

            // ============ Entity -> Response ============
            CreateMap<Projects, ProjectResponseDto>()
                .ForMember(d => d.CustomerName,
                    opt => opt.MapFrom(s => s.Customer != null ? s.Customer.FullName : null))
                .ForMember(d => d.CompanyName,
                    opt => opt.MapFrom(s => s.Customer != null ? s.Customer.CampanyName : null))
                .ForMember(d => d.ProjectManagerName,
                    opt => opt.MapFrom(s => s.ProjectManager != null ? s.ProjectManager.FullName : null))
                .ForMember(d => d.CreatedByName,
                    opt => opt.MapFrom(s => s.CreatedBy != null ? s.CreatedBy.FullName : null))
                .ForMember(d => d.StatusName,
                    opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.PriorityName,
                    opt => opt.MapFrom(s => s.Priority.ToString()))
                .ForMember(d => d.Engineers,
                    opt => opt.MapFrom(s => s.ProjectEngineers))
                .ForMember(d => d.TeamMembersCount,
                    opt => opt.MapFrom(s => s.ProjectEngineers.Count))
                .ForMember(d => d.Progress,
                    opt => opt.MapFrom(s => s.Tasks.Count() == 0
                        ? 0
                        : s.Tasks.Count(t => t.Status == StatusOfTask.Completed) * 100 / s.Tasks.Count()));
        }
    }
}
