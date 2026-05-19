using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.Appointments;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Application.Mapping
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<CreateAppointmentDto,Appointment>()
                .ForMember(d=>d.Status, opt => opt.MapFrom(_ => AppointmentStatus.Scheduled))
                    .ForMember(d => d.AssignedToId,
                    opt => opt.MapFrom(src => src.AssignedToUserId));

            CreateMap<Appointment, AppointmentResponseDto>()
            .ForMember(d => d.TypeNameAr, opt => opt.MapFrom(s => GetTypeNameAr(s.Type)))
            .ForMember(d => d.PriorityNameAr, opt => opt.MapFrom(s => GetPriorityNameAr(s.Priority)))
            .ForMember(d => d.StatusNameAr, opt => opt.MapFrom(s => GetStatusNameAr(s.Status)))
            
          .ForMember(d => d.CustomerFullName,
        opt => opt.MapFrom(s => s.Customer != null ? s.Customer.FullName : null))
    // ⭐ ربط اسم الموظف (لأنه راجع الايميل دلوقتي)
    .ForMember(d => d.AssignedToUserName,
        opt => opt.MapFrom(s =>
            s.AssignedTo != null && !string.IsNullOrEmpty(s.AssignedTo.FullName)
                ? s.AssignedTo.FullName
                : (s.AssignedTo != null ? s.AssignedTo.UserName : string.Empty)));


            CreateMap<UpdateAppointmentDto, Appointment>()
    .ForMember(d => d.AssignedToId, opt => opt.MapFrom(s => s.AssignedToUserId));
        }
        private static string GetPriorityNameAr(AppointmentPriority p) => p switch
        {
            AppointmentPriority.Low => "منخفضة",
            AppointmentPriority.Medium => "متوسطة",
            AppointmentPriority.High => "عالية",
            AppointmentPriority.Urgent => "عاجلة",
            _ => "منخفضة"
        };
        private static string GetTypeNameAr(AppointmentType t) => t switch
        {
            AppointmentType.Meeting => "اجتماع",
            AppointmentType.Demo => "عرض تجريبي",
            AppointmentType.Call => "مكالمة",
            AppointmentType.FollowUp => "متابعة",

            AppointmentType.Presentation => "عرض تقديمي",
            AppointmentType.Negotiation => "تفاوض",
            AppointmentType.ContractSigning => "توقيع عقد",
            AppointmentType.Support => "دعم فني",
            AppointmentType.Other => "أخرى"


        };
        private static string GetStatusNameAr(AppointmentStatus s) => s switch
        {
            AppointmentStatus.Scheduled => "مجدول",
            AppointmentStatus.NoShow => "لم يحضر",
            AppointmentStatus.Completed => "مكتمل",
            AppointmentStatus.Cancelled => "ملغي",
            AppointmentStatus.Postponed => "مؤجل",
            _ => "مجدول"
        };
    }
}
       
    