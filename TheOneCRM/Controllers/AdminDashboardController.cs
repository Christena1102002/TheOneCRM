using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces.IAdminDashboard;
using TheOneCRM.Domain.Models.Constants;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/AdminDashboard
        // 4 كروت: المشتريين، غير المشتريين، المشاريع النشطة، المهام المعلقة
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _dashboardService.GetDashboardAsync();
            return Ok(new ApiResponse(200, "Admin dashboard retrieved successfully", result));
        }
    }
}
