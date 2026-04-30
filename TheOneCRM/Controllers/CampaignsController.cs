using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces.ICampaign;

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
    }
}
