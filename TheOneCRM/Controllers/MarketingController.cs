using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

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
            return StatusCode(200,
             new ApiResponse(200, "Get All Statistics successfully", result));
        }
        [HttpGet("DashboardsStatistics")]
        public async Task<IActionResult> DashboardStats()
        {
            var result = await _marketingService.GetDashboardStats();
            return StatusCode(200,
              new ApiResponse(200, "Get All Statistics successfully", result));
        }
        // MarketingController
        [HttpGet("potentialCustomersLastdays")]
        public async Task<IActionResult> GetPotentialCustomersLastDays()
        {
            var result = await _marketingService.GetPotentialCustomersLast7DaysAsync();
            return StatusCode(200,
                new ApiResponse(200, "Get All Potential CustomersLast 7Days successfully", result));
        }

        [HttpGet("sourcePerformance")]
        public async Task<IActionResult> GetSourcePerformance()
        {
            var result = await _marketingService.GetSourcePerformanceAsync();
            return StatusCode(200,
                new ApiResponse(200, "Get All Source Performance successfully", result));
        }
    }
}
