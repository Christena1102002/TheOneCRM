using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class CampaignMapping:Profile
    {
        public CampaignMapping()
        {
            CreateMap<CampaignCountry, CampaignCountryDto>();

            CreateMap<Campaigns, CampaignDropdownDto>();
            CreateMap<Campaigns, CampaignResponseDto>()
              .ForMember(dest => dest.SourceName,
                  opt => opt.MapFrom(src => src.ChannelSource.Name));
          
            
            
            CreateMap<CreateCampaignDto, Campaigns>()
                .ForMember(dest => dest.ChannelSourceId, opt => opt.MapFrom(src => src.ChannelSourceId))
    .ForMember(dest => dest.CampaignCountries, opt => opt.Ignore()); 
                 

            CreateMap<Campaigns, CampaignListItemDto>()
        //.ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
        .ForMember(d => d.ChannelSource, o => o.MapFrom(s => s.ChannelSource.Name)) // عدّل حسب اسم البروبرتي عندك
        //.ForMember(d => d.DurationDays, o => o.MapFrom(s => s.DurationDays))
        //.ForMember(d => d.Countries, o => o.MapFrom(s => s.Countries.Select(cc => cc.name)))
        .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
          .ForMember(dest => dest.CustomersCount,
        opt => opt.MapFrom(src => src.Customers.Count()))
           .ForMember(d => d.Countries, o => o.MapFrom(s => s.CampaignCountries));

            CreateMap<Campaigns, CampaignDetailsDto>()
                .ForMember(d=>d.Gender,o=>o.MapFrom(s => s.Gender.ToString()))
                    .ForMember(dest => dest.ChannelSource,
        opt => opt.MapFrom(src => src.ChannelSource.Name))
    .ForMember(dest => dest.appUserName,
        opt => opt.MapFrom(src => src.appUser.UserName))
    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status.ToString()))
        .ForMember(dest => dest.Countries,
        opt => opt.MapFrom(src => src.CampaignCountries))
       

      .ForMember(d => d.DailyBudget, o => o.Ignore())
    .ForMember(d => d.Spent, o => o.Ignore())
    .ForMember(d => d.Remaining, o => o.Ignore())
    .ForMember(d => d.SpentPercentage, o => o.Ignore())
    .ForMember(d => d.DaysElapsed, o => o.Ignore())
    .ForMember(d => d.DaysRemaining, o => o.Ignore());

        //    CreateMap<Campaigns, CampaignDetailsDto>()
        //        .ForMember(dest => dest.ChannelSource,
        //opt => opt.MapFrom(src => src.ChannelSource.Name));


            CreateMap<Campaigns, CampaignDashboardDto>();


            CreateMap<UpdateCampaignDto, Campaigns>()
    .ForAllMembers(opt =>
        opt.Condition((src, dest, srcMember) => srcMember != null));
    


            CreateMap<CampaignCountry, CampaignCountryDto>();
        }

    }
       
}
