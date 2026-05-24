using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces.IAnalytics;
using TheOneCRM.Domain.Models.Constants;

namespace TheOneCRM.API.Controllers
{
    // تحليلات المطورين — للأدمن فقط
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class DeveloperAnalyticsController : ControllerBase
    {
        private readonly IDeveloperAnalyticsService _analyticsService;

        public DeveloperAnalyticsController(IDeveloperAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/DeveloperAnalytics/Summary — الكروت + المميّزون
        [HttpGet("Summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _analyticsService.GetSummaryAsync();
            return Ok(new ApiResponse(200, "Analytics summary retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/DeveloperStats — إحصائيات المطورين الفردية
        [HttpGet("DeveloperStats")]
        public async Task<IActionResult> GetDeveloperStats()
        {
            var result = await _analyticsService.GetDeveloperStatsAsync();
            return Ok(new ApiResponse(200, "Developer stats retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/Charts — إنجاز المهام عبر الزمن + تقدّم المشاريع
        [HttpGet("Charts")]
        public async Task<IActionResult> GetCharts()
        {
            var result = await _analyticsService.GetChartsAsync();
            return Ok(new ApiResponse(200, "Analytics charts retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/BugAnalytics — تحليلات الأخطاء
        [HttpGet("BugAnalytics")]
        public async Task<IActionResult> GetBugAnalytics()
        {
            var result = await _analyticsService.GetBugAnalyticsAsync();
            return Ok(new ApiResponse(200, "Bug analytics retrieved successfully", result));
        }
    }
}
