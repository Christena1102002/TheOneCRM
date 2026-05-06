using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }


        // GET: api/campaigns/dropdown
        [SwaggerOperation(Summary = "GET:campaigns/dropdown in marketing")]
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetForDropdown()
        {
            var result = await _campaignService.GetCampaignsForDropdownAsync();
            return StatusCode(200,
                 new ApiResponse(200, "get Campaign successfully", result));
        }
        [HttpPost]
        public async Task<ActionResult> CreateCampaign(CreateCampaignDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _campaignService.CreateCampaignAsync(dto, userId);
            return StatusCode(200,
               new ApiResponse(200, "get Campaign successfully", result));
        }
        [HttpGet("GetAllCampaigns")]
        public async Task<IActionResult> GetAllCampaigns([FromQuery] CampaignPaginationParams paginationParams)
        {
            var result = await _campaignService.GetAllCampaignsAsync(paginationParams);
            return StatusCode(200,
                 new ApiResponse(200, "get all Campaigns successfully", result));
        }
        [HttpGet("{id}/getCampaignById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _campaignService.GetCampaignByIdAsync(id);
            return StatusCode(200,
                   new ApiResponse(200, "Get Campaign By Id successfully", result));
        }
        [HttpDelete("{id}/deleteCampaign")]
        public async Task<IActionResult> Delete(int id)
        {
            await _campaignService.DeleteCampaignAsync(id);
            return StatusCode(200,
                new ApiResponse(200, "Campaign deleted successfully"));
        }
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _campaignService.ToggleCampaignStatusAsync(id);

            return Ok(new ApiResponse(200, "Campaign status toggled successfully", result));
        }
        [HttpGet("Statistics-Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _campaignService.GetCampaignsDashboardAsync();

            return Ok(new ApiResponse(200, "Dashboard retrieved", result));
        }
    }
}
