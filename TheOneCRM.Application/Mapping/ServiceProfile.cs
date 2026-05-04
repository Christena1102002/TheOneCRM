using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.ServicesDtos;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<CreateServiceDto, Service>();
            CreateMap<Service, ServiceDto>();
            CreateMap<UpdateServiceDto, Service>();
        }
    }
}
