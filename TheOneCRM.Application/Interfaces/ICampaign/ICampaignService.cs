using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;

namespace TheOneCRM.Application.Interfaces.ICampaign
{
    public interface ICampaignService
    {
        Task<IReadOnlyList<CampaignDropdownDto>> GetCampaignsForDropdownAsync();
        Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto, string userId);
        // ICampaignService.cs
      
        Task<Pagination<CampaignListItemDto>> GetAllCampaignsAsync(CampaignPaginationParams paginationParams, string? ownerId);
        Task<CampaignDetailsDto> GetCampaignByIdAsync(int id, string? ownerId);
        Task DeleteCampaignAsync(int id);
        Task<CampaignResponseDto> ToggleCampaignStatusAsync(int id);
        Task<List<CampaignDashboardDto>> GetCampaignsDashboardAsync(string? ownerId);
        Task<List<CampaignPerformanceRowDto>> GetCampaignPerformance(string? ownerId);
        // Service Interface
        Task<CampaignResponseDto> UpdateCampaignAsync(int id, UpdateCampaignDto dto, string userId);
    }
}
