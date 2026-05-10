using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.Application.Interfaces;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingController : ControllerBase
    {
        private readonly IMarketingService _marketingService;

        public MarketingController(IMarketingService marketingService)
        {
            _marketingService = marketingService;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _marketingService.GetStatisticsAsync();
            return Ok(result);
        }
    }
}
