using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;

namespace TheOneCRM.Application.Interfaces.ICampaign
{
    public interface ICampaignService
    {
        Task<IReadOnlyList<CampaignDropdownDto>> GetCampaignsForDropdownAsync();
        Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto, string userId);
    }
}
