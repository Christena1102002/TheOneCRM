using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.Contracts;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class ContractProfile:Profile
    {
        public ContractProfile()
        {
            // ============ Create: DTO -> Entity ============
            CreateMap<CreateContractDto, Contract>();
            CreateMap<Customer, CustomerLookupDto>();
            CreateMap<Contract, ContractResponseDto>()
              .ForMember(d => d.CustomerName,
                  opt => opt.MapFrom(s => s.Customer != null ? s.Customer.FullName : null))
              .ForMember(d => d.CompanyName,
                  opt => opt.MapFrom(s => s.Customer != null ? s.Customer.CampanyName : null))
              .ForMember(d => d.Phone,
                  opt => opt.MapFrom(s => s.Customer != null ? s.Customer.Phone : null))
              .ForMember(d => d.Address,
                  opt => opt.MapFrom(s => s.Customer != null ? s.Customer.Address : null))
              .ForMember(d => d.CreatedByName,
                  opt => opt.MapFrom(s => s.CreatedBy != null ? s.CreatedBy.FullName : null))
              .ForMember(d => d.StatusName,
                  opt => opt.MapFrom(s => s.Status.ToString()))
              .ForMember(d=>d.CreatedAt,opt=>opt.MapFrom(s=>s.CreatedAt));

            CreateMap<UpdateContractDto, Contract>()
    .ForMember(d => d.Id, opt => opt.Ignore())
    .ForMember(d => d.CustomerId, opt => opt.Ignore())
    .ForMember(d => d.CreatedById, opt => opt.Ignore())
    .ForMember(d => d.Customer, opt => opt.Ignore())
    .ForMember(d => d.CreatedBy, opt => opt.Ignore())
    .ForMember(d => d.Status, opt => opt.Ignore());
        }
    }
}
