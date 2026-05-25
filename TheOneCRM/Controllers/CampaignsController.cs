using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
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

        // الأدمن يشوف الكل (null)، والماركتينج يشوف حملاته هو بس
        private string? OwnerId() => User.IsAdmin() ? null : User.GetUserId();


        // GET: api/campaigns/dropdown
        [SwaggerOperation(Summary = "GET:campaigns/dropdown in marketing")]
        [HttpGet("dropdown")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing},{UserRoles.Sales}")]
        public async Task<IActionResult> GetForDropdown()
        {
            var result = await _campaignService.GetCampaignsForDropdownAsync();
            return StatusCode(200,
                 new ApiResponse(200, "get Campaign successfully", result));
        }
        [HttpPost("CreateCampaign")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<ActionResult> CreateCampaign(CreateCampaignDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _campaignService.CreateCampaignAsync(dto, userId);
            return StatusCode(200,
               new ApiResponse(200, "get Campaign successfully", result));
        }
        [HttpPut("UpdateCampaign/{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<ActionResult> UpdateCampaign(int id, [FromBody] UpdateCampaignDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _campaignService.UpdateCampaignAsync(id, dto, userId);

            return StatusCode(200,
                new ApiResponse(200, "Campaign updated successfully", result));
        }
        [HttpGet("GetAllCampaigns")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<IActionResult> GetAllCampaigns([FromQuery] CampaignPaginationParams paginationParams)
        {
            var result = await _campaignService.GetAllCampaignsAsync(paginationParams, OwnerId());
            return StatusCode(200,
                 new ApiResponse(200, "get all Campaigns successfully", result));
        }
        [HttpGet("{id}/getCampaignById")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _campaignService.GetCampaignByIdAsync(id, OwnerId());
            return StatusCode(200,
                   new ApiResponse(200, "Get Campaign By Id successfully", result));
        }
        [HttpDelete("{id}/deleteCampaign")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _campaignService.DeleteCampaignAsync(id);
            return StatusCode(200,
                new ApiResponse(200, "Campaign deleted successfully"));
        }
        [HttpPatch("{id}/toggleStatus")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _campaignService.ToggleCampaignStatusAsync(id);

            return Ok(new ApiResponse(200, "Campaign status toggled successfully", result));
        }
        [HttpGet("StatisticsDashboard")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _campaignService.GetCampaignsDashboardAsync(OwnerId());

            return Ok(new ApiResponse(200, "Dashboard retrieved", result));
        }
        [HttpGet("dropdownCountries")]
        [Authorize]
        public IActionResult GetCountries()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/countries.json");
            var json = System.IO.File.ReadAllText(path);

            return Content(json, "application/json");
        }
        [HttpGet("CampaignPerformance")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
        public async Task<IActionResult> CampaignPerformance()
        {
            var result = await _campaignService.GetCampaignPerformance(OwnerId());
            return Ok(new ApiResponse(200, "Dashboard retrieved", result));
        }

        //[HttpGet("getLeadCustomer")]

        //public async Task<IActionResult> GetAll([FromQuery] CustomerPaginationParams paginationParams)
        //{
        //    var result = await _campaignService.GetAllCustomersAsync(paginationParams);
        //    return StatusCode(200,
        //          new ApiResponse(200, "get customers successfully", result));
        //}
    }
}
