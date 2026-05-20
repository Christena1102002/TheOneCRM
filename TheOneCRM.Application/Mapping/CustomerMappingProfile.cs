using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;

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
                .ForMember(dest => dest.Notes, opt => opt.Ignore()); 


            CreateMap<Customer, CustomerResponseDto>()
                 .ForMember(dest => dest.NoteMarketing,
        opt => opt.MapFrom(src =>
            src.Notes
                .OrderByDescending(n => n.Id)
                .Select(n => n.NoteMarketing)
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))))

    .ForMember(dest => dest.NoteSales,
        opt => opt.MapFrom(src =>
            src.Notes
                .OrderByDescending(n => n.Id)
                .Select(n => n.NoteSales)
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))))

    .ForMember(dest => dest.NoteSupport,
        opt => opt.MapFrom(src =>
            src.Notes
                .OrderByDescending(n => n.Id)
                .Select(n => n.NoteSupport)
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))));
            CreateMap<CustomerNote, CustomerNoteResponseDto>()
    .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.FullName))
    // Notes
    .ForMember(d => d.NoteMarketing,
        o => o.MapFrom(s => s.NoteMarketing))

    .ForMember(d => d.NoteSales,
        o => o.MapFrom(s => s.NoteSales))

    .ForMember(d => d.NoteSupport,
        o => o.MapFrom(s => s.NoteSupport))

    // Roles
    .ForMember(d => d.MarketingRole,
        o => o.MapFrom(s =>
            !string.IsNullOrWhiteSpace(s.NoteMarketing) ? "Marketing" : null))

    .ForMember(d => d.SalesRole,
        o => o.MapFrom(s =>
            !string.IsNullOrWhiteSpace(s.NoteSales) ? "Sales" : null))

    .ForMember(d => d.SupportRole,
        o => o.MapFrom(s =>
            !string.IsNullOrWhiteSpace(s.NoteSupport) ? "Support" : null))
    // Marketing
    .ForMember(d => d.NoteMarketingName,
        o => o.MapFrom(s => s.MarketingCreatedBy != null ? s.MarketingCreatedBy.FullName : null))

    // Sales
    .ForMember(d => d.NoteSalesName,
        o => o.MapFrom(s => s.SalesCreatedBy != null ? s.SalesCreatedBy.FullName : null))

    // Support
    .ForMember(d => d.NoteSupportName,
        o => o.MapFrom(s => s.SupportCreatedBy != null ? s.SupportCreatedBy.FullName : null));



            CreateMap<Customer, CustomerListItemDto>()
     .ForMember(d => d.Status,
         o => o.MapFrom(s => s.status.ToString()))
     .ForMember(d => d.CampaignName,
         o => o.MapFrom(s => s.campaigns != null ? s.campaigns.Name : null))

          .ForMember(d => d.SalesPersonId,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Sales")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUserId)
            .FirstOrDefault()))

     .ForMember(d => d.SupportPersonId,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Support")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUserId)
            .FirstOrDefault()))

       .ForMember(d => d.SupportPersonName,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Support")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUser != null ? a.ToUser.FullName : null)
            .FirstOrDefault()))

         .ForMember(d => d.SalesPersonName,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Sales")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUser != null ? a.ToUser.FullName : null)
            .FirstOrDefault()))

     .ForMember(d => d.Services,
         o => o.MapFrom(s => s.customerServices.Select(cs => cs.Service.NameAr).ToList()))
      .ForMember(dest => dest.Source,
                opt => opt.MapFrom(src => src.campaigns.ChannelSource.Name))
      .ForMember(dest => dest.NotBuyerReason,
        opt => opt.MapFrom(src =>
            src.status== StatusOfCustomers.NotBuyer ? src.NotBuyingReason : null));
      


            CreateMap<Customer, CustomerDetailsDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => (int)s.status))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.status.ToString()))
                .ForMember(d => d.CampaignName,
         o => o.MapFrom(s => s.campaigns != null ? s.campaigns.Name : null))
             //       .ForMember(d => d.SalesPersonName,
             //o => o.MapFrom(s => s.AssignedTo != null ? s.AssignedTo.FullName : null))
             .ForMember(d => d.SalesPersonId,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Sales")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUserId)
            .FirstOrDefault()))

     .ForMember(d => d.SupportPersonId,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Support")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUserId)
            .FirstOrDefault()))

       .ForMember(d => d.SupportPersonName,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Support")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUser != null ? a.ToUser.FullName : null)
            .FirstOrDefault()))

         .ForMember(d => d.SalesPersonName,
        o => o.MapFrom(s => s.AssignmentHistory
            .Where(a => a.ToRole == "Sales")
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => a.ToUser != null ? a.ToUser.FullName : null)
            .FirstOrDefault()))
     .ForMember(d => d.Services,
         o => o.MapFrom(s => s.customerServices.Select(cs => cs.Service.NameAr).ToList()))
      .ForMember(dest => dest.Source,
                opt => opt.MapFrom(src => src.campaigns.ChannelSource.Name))
      .ForMember(d=>d.SalesPersonId,o=>o.MapFrom(s=>s.AssignedToId))
      .ForMember(d=>d.CampaignId,o=>o.MapFrom(s=>s.compaignId))
      .ForMember(d=>d.AssignedAt,o=>o.MapFrom(s=>s.CreatedAt))
      ;

   

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


            CreateMap<CustomerNote, CustomerNoteResponseDto>()
                 // Notes
    .ForMember(d => d.NoteMarketing,
        o => o.MapFrom(s => s.NoteMarketing))

    .ForMember(d => d.NoteSales,
        o => o.MapFrom(s => s.NoteSales))

    .ForMember(d => d.NoteSupport,
        o => o.MapFrom(s => s.NoteSupport))

    // Roles
    .ForMember(d => d.MarketingRole,
        o => o.MapFrom(s =>
            !string.IsNullOrWhiteSpace(s.NoteMarketing) ? "Marketing" : null))

    .ForMember(d => d.SalesRole,
        o => o.MapFrom(s =>
            !string.IsNullOrWhiteSpace(s.NoteSales) ? "Sales" : null))

    .ForMember(d => d.SupportRole,
        o => o.MapFrom(s =>
            !string.IsNullOrWhiteSpace(s.NoteSupport) ? "Support" : null))
            .ForMember(dest => dest.CustomerName,
        opt => opt.MapFrom(src => src.Customer.FullName))
    // Marketing
    .ForMember(d => d.NoteMarketingName,
        o => o.MapFrom(s => s.MarketingCreatedBy != null ? s.MarketingCreatedBy.FullName : null))

    // Sales
    .ForMember(d => d.NoteSalesName,
        o => o.MapFrom(s => s.SalesCreatedBy != null ? s.SalesCreatedBy.FullName : null))

    // Support
    .ForMember(d => d.NoteSupportName,
        o => o.MapFrom(s => s.SupportCreatedBy != null ? s.SupportCreatedBy.FullName : null));

            

        }
    }
}
