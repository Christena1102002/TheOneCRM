using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notifications, NotificationResponseDto>();
        }
    }
}
