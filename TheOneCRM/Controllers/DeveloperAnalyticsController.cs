using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.IAnalytics;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Analytics;

namespace TheOneCRM.API.Controllers
{
    // تحليلات المطورين — الأدمن يشوف الكل (أو يفلتر بمطوّر)، والمطوّر يشوف بياناته هو بس
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Developer}")]
    public class DeveloperAnalyticsController : ControllerBase
    {
        private readonly IDeveloperAnalyticsService _analyticsService;

        public DeveloperAnalyticsController(IDeveloperAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // الأدمن: يستخدم فلتر المطوّر اللي بعته (null = الكل). المطوّر: دايماً بياناته هو.
        private DeveloperAnalyticsParams Resolve(DeveloperAnalyticsParams p)
        {
            if (!User.IsAdmin())
                p.DeveloperId = User.GetUserId();
            return p;
        }

        // GET: api/DeveloperAnalytics — كل التحليلات في رد واحد
        // الأدمن: يشوف الكل (أو يفلتر بـ developerId). المطوّر: بياناته هو بس.
        [HttpGet]
        public async Task<IActionResult> GetFullAnalytics([FromQuery] DeveloperAnalyticsParams p)
        {
            var result = await _analyticsService.GetFullAnalyticsAsync(Resolve(p));
            return Ok(new ApiResponse(200, "Analytics retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/Summary — الكروت + المميّزون
        [HttpGet("Summary")]
        public async Task<IActionResult> GetSummary([FromQuery] DeveloperAnalyticsParams p)
        {
            var result = await _analyticsService.GetSummaryAsync(Resolve(p));
            return Ok(new ApiResponse(200, "Analytics summary retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/DeveloperStats — إحصائيات المطورين الفردية
        [HttpGet("DeveloperStats")]
        public async Task<IActionResult> GetDeveloperStats([FromQuery] DeveloperAnalyticsParams p)
        {
            var result = await _analyticsService.GetDeveloperStatsAsync(Resolve(p));
            return Ok(new ApiResponse(200, "Developer stats retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/Charts — إنجاز المهام عبر الزمن + تقدّم المشاريع
        [HttpGet("Charts")]
        public async Task<IActionResult> GetCharts([FromQuery] DeveloperAnalyticsParams p)
        {
            var result = await _analyticsService.GetChartsAsync(Resolve(p));
            return Ok(new ApiResponse(200, "Analytics charts retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/BugAnalytics — تحليلات الأخطاء
        [HttpGet("BugAnalytics")]
        public async Task<IActionResult> GetBugAnalytics([FromQuery] DeveloperAnalyticsParams p)
        {
            var result = await _analyticsService.GetBugAnalyticsAsync(Resolve(p));
            return Ok(new ApiResponse(200, "Bug analytics retrieved successfully", result));
        }

        // GET: api/DeveloperAnalytics/ProjectOptions — مشاريع المطوّر المختار (للـ dropdown)
        // الأدمن: يبعت developerId (null = كل المشاريع). المطوّر: مشاريعه هو.
        [HttpGet("ProjectOptions")]
        public async Task<IActionResult> GetProjectOptions([FromQuery] string? developerId = null)
        {
            var effectiveId = User.IsAdmin() ? developerId : User.GetUserId();
            var result = await _analyticsService.GetProjectOptionsAsync(effectiveId);
            return Ok(new ApiResponse(200, "Project options retrieved successfully", result));
        }
    }
}
