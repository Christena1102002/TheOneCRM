using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.SupportTickets;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class SupportTicketProfile : Profile
    {
        public SupportTicketProfile()
        {
            // ============ Create: DTO -> Entity ============
            CreateMap<CreateSupportTicketDto, SupportTickets>()
                .ForMember(d => d.priority, opt => opt.MapFrom(s => s.Priority));

            // ============ Update: DTO -> Entity ============
            CreateMap<UpdateSupportTicketDto, SupportTickets>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.TicketNumber, opt => opt.Ignore())
                .ForMember(d => d.CustomerId, opt => opt.Ignore())
                .ForMember(d => d.Customer, opt => opt.Ignore())
                .ForMember(d => d.Service, opt => opt.Ignore())
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.priority, opt => opt.MapFrom(s => s.Priority));

            // ============ Entity -> Response ============
            CreateMap<SupportTickets, SupportTicketResponseDto>()
                .ForMember(d => d.Priority, opt => opt.MapFrom(s => s.priority))
                .ForMember(d => d.PriorityName, opt => opt.MapFrom(s => s.priority.ToString()))
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.CustomerName,
                    opt => opt.MapFrom(s => s.Customer != null ? s.Customer.FullName : null))
                .ForMember(d => d.CompanyName,
                    opt => opt.MapFrom(s => s.Customer != null ? s.Customer.CampanyName : null))
                .ForMember(d => d.ServiceName,
                    opt => opt.MapFrom(s => s.Service != null ? s.Service.NameAr : null))
                .ForMember(d => d.CreatedByName,
                    opt => opt.MapFrom(s => s.CreatedBy != null ? s.CreatedBy.FullName : null));
        }
    }
}
