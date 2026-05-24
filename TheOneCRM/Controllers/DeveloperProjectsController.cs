using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.IProjects;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Projects;

namespace TheOneCRM.API.Controllers
{
    // مشاريع الـ developer اللي هو معيّن عليها فقط
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Developer)]
    public class DeveloperProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public DeveloperProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/DeveloperProjects/GetMyProjects
        [HttpGet("GetMyProjects")]
        public async Task<IActionResult> GetMyProjects([FromQuery] ProjectParams p)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _projectService.GetMyProjectsAsync(p, userId);
            return Ok(new ApiResponse(200, "Projects retrieved successfully", result));
        }

        // GET: api/DeveloperProjects/GetMyProjectById/5
        [HttpGet("GetMyProjectById/{id:int}")]
        public async Task<IActionResult> GetMyProject(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var project = await _projectService.GetMyProjectByIdAsync(id, userId);
            return Ok(new ApiResponse(200, "Project retrieved successfully", project));
        }

        // GET: api/DeveloperProjects/GetMyActiveProjects — مشاريع المطوّر النشطة
        [HttpGet("GetMyActiveProjects")]
        public async Task<IActionResult> GetMyActiveProjects([FromQuery] ProjectParams p)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _projectService.GetMyActiveProjectsAsync(p, userId);
            return Ok(new ApiResponse(200, "Active projects retrieved successfully", result));
        }
    }
}
