using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.GoalDtos;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<CreateGoalDto, Goal>()
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.AssignedTo, opt => opt.Ignore());
        }
    }
}
