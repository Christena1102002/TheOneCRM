using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public CampaignService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<CampaignDropdownDto>> GetCampaignsForDropdownAsync()
        {
            var spec = new ActiveCampaignsSpec();
            var campaigns = await _unitOfWork.Repository<Campaigns>().ListWithSelectAsync(spec,x=>new CampaignDropdownDto {Id=x.Id,Name=x.Name });
            return campaigns;
            //return _mapper.Map<IReadOnlyList<CampaignDropdownDto>>(campaigns);
        }

        public async Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var isMarketing = await _userManager.IsInRoleAsync(user, "Marketing");

            if (!isMarketing)
                throw new UnauthorizedAccessException("Only Marketing users can create campaigns");
            // 1️⃣ Validate Channel Source
            var source = await _unitOfWork.Repository<ChannelSource>()
                .GetByIdAsync(dto.ChannelSourceId.Value);

            if (source == null)
                throw new KeyNotFoundException("Invalid Channel Source");

            // 2️⃣ Map main entity
            var campaign = _mapper.Map<Campaigns>(dto);
            campaign.AppUserId = userId;
            campaign.ChannelSourceId = (int)dto.ChannelSourceId;
            if (dto.Countries != null && dto.Countries.Any())
            {
                campaign.Countries = dto.Countries
                    .Select(c => new CampaignCountry
                    {
                        Id = c
                    })
                    .ToList();
            }
            // 3️⃣ Dates validation
            //if (dto.EndDate < dto.StartDate)
            //    throw new InvalidOperationException("EndDate must be after StartDate");

            // 4️⃣ Save
            await _unitOfWork.Repository<Campaigns>().AddAsync(campaign);
            await _unitOfWork.SaveChangesAsync();

            // 5️⃣ IMPORTANT: reload with specification (for response)
            var spec = new CampaignWithDetailsSpec(campaign.Id);

            var createdCampaign = await _unitOfWork.Repository<Campaigns>()
                .GetEntityWithSpec(spec);

            // 6️⃣ Map response
            return _mapper.Map<CampaignResponseDto>(createdCampaign);
        }
    }
}
