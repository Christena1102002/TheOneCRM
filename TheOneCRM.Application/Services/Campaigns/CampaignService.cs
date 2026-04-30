using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;

namespace TheOneCRM.Application.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampaignService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CampaignDropdownDto>> GetCampaignsForDropdownAsync()
        {
            var spec = new ActiveCampaignsSpec();
            var campaigns = await _unitOfWork.Repository<Campaigns>().ListAsync(spec);

            return _mapper.Map<IReadOnlyList<CampaignDropdownDto>>(campaigns);
        }
    }
}
