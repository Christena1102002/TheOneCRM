using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces.IResources;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Resources;

namespace TheOneCRM.API.Controllers
{
    // إدارة الموارد وأعباء العمل — للأدمن (المدير) فقط
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class ResourceManagementController : ControllerBase
    {
        private readonly IResourceManagementService _resourceService;

        public ResourceManagementController(IResourceManagementService resourceService)
        {
            _resourceService = resourceService;
        }

        // GET: api/ResourceManagement/TeamWorkload
        // نظرة عامة على الفريق + الكروت (متاحون/محمّلون/المتوسط/الإجمالي)
        [HttpGet("TeamWorkload")]
        public async Task<IActionResult> GetTeamWorkload([FromQuery] WorkloadParams p)
        {
            var result = await _resourceService.GetTeamWorkloadOverviewAsync(p);
            return Ok(new ApiResponse(200, "Team workload retrieved successfully", result));
        }

        // GET: api/ResourceManagement/WorkloadDistribution
        // توزيع أحمال العمل لكل مطوّر
        [HttpGet("WorkloadDistribution")]
        public async Task<IActionResult> GetWorkloadDistribution()
        {
            var result = await _resourceService.GetWorkloadDistributionAsync();
            return Ok(new ApiResponse(200, "Workload distribution retrieved successfully", result));
        }
    }
}
