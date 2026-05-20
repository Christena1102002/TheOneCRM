using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.ICustomers;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public SalesController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("SalesDashboardStatistics")]
        [Authorize(Roles = "Sales,Admin,Support")]
        public async Task<IActionResult> GetSalesStats()
        {

            var CurrentUserId = User.GetUserId();
            if (string.IsNullOrEmpty(CurrentUserId))
                return Unauthorized();
            var stats = await _customerService.GetSalesDashboardStatsAsync(CurrentUserId);
            return Ok(stats);
        }
        [HttpGet("SalesCustomerStatusCount")]
        public async Task<ActionResult> GetCustomerStatusCount()
        {
            var currentUserId = User.GetUserId();
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            var result = await _customerService.GetCustomerCountByStatusAsync(currentUserId);
            return Ok(new ApiResponse(
                 200,
                 "Not buying reasons retrieved successfully",
                 result
             ));
        }
        [HttpGet("NotBuyingReasons")]
        public async Task<IActionResult> GetNotBuyingReasons()
        {
            var reasons = await _customerService.GetNotBuyingReasonsAsync();

            return Ok(new ApiResponse(
                200,
                "Not buying reasons retrieved successfully",
                reasons
            ));
        }
    }
}
