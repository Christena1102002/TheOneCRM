using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing}")]
    public class MarketingController : ControllerBase
    {
        private readonly IMarketingService _marketingService;

        public MarketingController(IMarketingService marketingService)
        {
            _marketingService = marketingService;
        }

        // الأدمن يشوف الكل (null)، الماركتينج يشوف اللي هو ضافهم
        private string? OwnerId() => User.IsAdmin() ? null : User.GetUserId();

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _marketingService.GetStatisticsAsync(OwnerId());
            return StatusCode(200,
             new ApiResponse(200, "Get All Statistics successfully", result));
        }
        [HttpGet("DashboardsStatistics")]
        public async Task<IActionResult> DashboardStats()
        {
            var result = await _marketingService.GetDashboardStats(OwnerId());
            return StatusCode(200,
              new ApiResponse(200, "Get All Statistics successfully", result));
        }
        // MarketingController
        [HttpGet("potentialCustomersLastdays")]
        public async Task<IActionResult> GetPotentialCustomersLastDays()
        {
            var result = await _marketingService.GetPotentialCustomersLast7DaysAsync(OwnerId());
            return StatusCode(200,
                new ApiResponse(200, "Get All Potential CustomersLast 7Days successfully", result));
        }

        [HttpGet("sourcePerformance")]
        public async Task<IActionResult> GetSourcePerformance()
        {
            var result = await _marketingService.GetSourcePerformanceAsync(OwnerId());
            return StatusCode(200,
                new ApiResponse(200, "Get All Source Performance successfully", result));
        }

        
    }
}
