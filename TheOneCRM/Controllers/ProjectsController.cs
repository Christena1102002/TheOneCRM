using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.IProjects;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Projects;

namespace TheOneCRM.API.Controllers
{
    // كل المشاريع وإدارتها — للأدمن (المدير) فقط
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class ManagerProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ManagerProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/ManagerProjects/customerLookup/5 — يرجّع اسم العميل واسم الشركة
        [HttpGet("customerLookup/{customerId:int}")]
        public async Task<IActionResult> GetCustomerLookup(int customerId)
        {
            var result = await _projectService.GetCustomerLookupAsync(customerId);
            return Ok(new ApiResponse(200, "Customer lookup retrieved successfully", result));
        }

        // POST: api/ManagerProjects/CreateProject
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject(CreateProjectDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var id = await _projectService.CreateProjectAsync(dto, userId);
            return Ok(new ApiResponse(200, "Project created successfully", new { id }));
        }

        // GET: api/ManagerProjects/GetProjectById/5
        [HttpGet("GetProjectById/{id:int}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            return Ok(new ApiResponse(200, "Project retrieved successfully", project));
        }

        // GET: api/ManagerProjects/GetProjects — كل المشاريع
        [HttpGet("GetProjects")]
        public async Task<IActionResult> GetProjects([FromQuery] ProjectParams p)
        {
            var result = await _projectService.GetProjectsAsync(p);
            return Ok(new ApiResponse(200, "Projects retrieved successfully", result));
        }

        // GET: api/ManagerProjects/GetActiveProjects — كل المشاريع النشطة
        [HttpGet("GetActiveProjects")]
        public async Task<IActionResult> GetActiveProjects([FromQuery] ProjectParams p)
        {
            var result = await _projectService.GetActiveProjectsAsync(p);
            return Ok(new ApiResponse(200, "Active projects retrieved successfully", result));
        }

        // PUT: api/ManagerProjects/UpdateProject/5
        [HttpPut("UpdateProject/{id:int}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto dto)
        {
            await _projectService.UpdateProjectAsync(id, dto);
            return Ok(new ApiResponse(200, "Project updated successfully"));
        }

        // DELETE: api/ManagerProjects/DeleteProject/5
        [HttpDelete("DeleteProject/{id:int}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return Ok(new ApiResponse(200, "Project deleted successfully"));
        }
    }
}
