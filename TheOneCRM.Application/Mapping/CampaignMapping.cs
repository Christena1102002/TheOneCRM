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
            CreateMap<Campaigns, CampaignDropdownDto>();
        }
    }
       
}
