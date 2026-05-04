using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class SourceProfile :Profile
    {
        public SourceProfile()
        {
            CreateMap<CreateChannelSourceDto, ChannelSource>();
            CreateMap<ChannelSource, ChannelSourceDto>();
        }
    }
}
