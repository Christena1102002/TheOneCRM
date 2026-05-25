using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.CompanySettings;
using Entity = TheOneCRM.Domain.Models.Entities.CompanySettings;

namespace TheOneCRM.Application.Mapping
{
    public class CompanySettingsProfile : Profile
    {
        public CompanySettingsProfile()
        {
            // Update/Create: DTO -> Entity (الشعار والأيقونة بيتظبطوا في الـ service)
            CreateMap<UpdateCompanySettingsDto, Entity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.LogoUrl, opt => opt.Ignore())
                .ForMember(d => d.FaviconUrl, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore());

            CreateMap<Entity, CompanySettingsResponseDto>();
        }
    }
}
