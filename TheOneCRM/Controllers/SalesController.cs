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
        [Authorize(Roles = "Sales,Admin")]
        public async Task<ActionResult> GetCustomerStatusCount()
        {
            var currentUserId = User.GetUserId();
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            var isAdmin = User.IsAdmin();
            var result = await _customerService.GetCustomerCountByStatusAsync(currentUserId, isAdmin);
            return Ok(new ApiResponse(
                 200,
                 "Customer status counts retrieved successfully",
                 result
             ));
        }
        [HttpGet("NotBuyingReasons")]
        [Authorize(Roles = "Admin,Sales")]
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
