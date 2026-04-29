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
                .ForMember(dest => dest.customerServices, opt => opt.Ignore()); // هنعملها يدوي

            CreateMap<Customer, CustomerResponseDto>();
        }
    }
}
