using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.CampanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.compaignId, opt => opt.MapFrom(src => src.CampaignId))
                .ForMember(dest => dest.customerServices, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore()); ; // هنعملها يدوي
            CreateMap<CustomerNote, CustomerNoteResponseDto>()
             .ForMember(d => d.CustomerName,
                 o => o.MapFrom(s => s.Customer != null ? s.Customer.FullName : null))
             .ForMember(d => d.CreatedByName,
                 o => o.MapFrom(s => s.CreatedBy != null ? s.CreatedBy.UserName : null));
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.CustomerNotes, opt => opt.MapFrom(src => src.Notes));
            CreateMap<CustomerNote, CustomerNoteResponseDto>()
    .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.FullName))
    .ForMember(d => d.CreatedByName, o => o.MapFrom(s => s.CreatedBy.UserName));
            CreateMap<Customer, CustomerListItemDto>()
     .ForMember(d => d.Status,
         o => o.MapFrom(s => s.status.ToString()))
     .ForMember(d => d.CampaignName,
         o => o.MapFrom(s => s.campaigns != null ? s.campaigns.Name : null))
     .ForMember(d => d.SalesPersonName,
         o => o.MapFrom(s => s.AssignedTo != null ? s.AssignedTo.UserName : null))
     .ForMember(d => d.Services,
         o => o.MapFrom(s => s.customerServices.Select(cs => cs.Service.NameAr).ToList()))
      .ForMember(dest => dest.Source,
                opt => opt.MapFrom(src => src.campaigns.ChannelSource.Name));


            CreateMap<Customer, CustomerDetailsDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => (int)s.status))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.status.ToString()))
                .ForMember(d => d.CampaignName,
         o => o.MapFrom(s => s.campaigns != null ? s.campaigns.Name : null))
                .ForMember(d => d.SalesPersonName,
         o => o.MapFrom(s => s.AssignedTo != null ? s.AssignedTo.UserName : null))
     .ForMember(d => d.Services,
         o => o.MapFrom(s => s.customerServices.Select(cs => cs.Service.NameAr).ToList()))
      .ForMember(dest => dest.Source,
                opt => opt.MapFrom(src => src.campaigns.ChannelSource.Name))
      .ForMember(d=>d.SalesPersonId,o=>o.MapFrom(s=>s.AssignedToId))
      .ForMember(d=>d.CampaignId,o=>o.MapFrom(s=>s.compaignId))
      .ForMember(d=>d.AssignedAt,o=>o.MapFrom(s=>s.CreatedAt));

   

            CreateMap<UpdateCustomerDto, Customer>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.campaigns, o => o.Ignore())
        .ForMember(d => d.AssignedTo, o => o.Ignore())
        .ForMember(d => d.AssignedToId, o => o.Ignore())
        .ForMember(d => d.status, o => o.Ignore())
        .ForMember(d => d.CreatedAt, o => o.Ignore());

            CreateMap<Customer, CustomerDropdownDto>()
                .ForMember(d => d.Services,
         o => o.MapFrom(s => s.customerServices.Select(cs => cs.Service.NameAr).ToList()));




            //// الحالة
            //.ForMember(d => d.Status,
            //    o => o.MapFrom(s => (int)s.status))
            //.ForMember(d => d.StatusName,
            //    o => o.MapFrom(s => s.status.ToString()))

            //// الحملة
            //.ForMember(d => d.CampaignName,
            //    o => o.MapFrom(s => s.campaigns != null ? s.campaigns.Name : null))

            // المندوب
            //.ForMember(d => d.SalesPersonId,
            //    o => o.MapFrom(s => s.AssignedToId))
            //.ForMember(d => d.SalesPersonName,
            //    o => o.MapFrom(s => s.AssignedTo != null ? s.AssignedTo.UserName : null))

            //// الخدمات: IDs
            //.ForMember(d => d.ServiceIds,
            //    o => o.MapFrom(s => s.customerServices.Select(cs => cs.ServiceId).ToList()))

            //// الخدمات: Objects
            //.ForMember(d => d.Services,
            //    o => o.MapFrom(s => s.customerServices.Select(cs => new ServiceItemDto
            //    {
            //        Id = cs.Service.Id,
            //        Name = cs.Service.NameAr
            //    }).ToList()));

            //        CreateMap<Service, ServiceItemDto>();
            //    }


        }
    }
}
