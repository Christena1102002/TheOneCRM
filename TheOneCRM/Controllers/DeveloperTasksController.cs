using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.ITasks;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Tasks;

namespace TheOneCRM.API.Controllers
{
    // مهام الـ developer — بيشوف مهامه بس ويغيّر حالتها
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Developer)]
    public class DeveloperTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public DeveloperTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/DeveloperTasks/GetMyTasks
        [HttpGet("GetMyTasks")]
        public async Task<IActionResult> GetMyTasks([FromQuery] TaskParams p)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _taskService.GetMyTasksAsync(p, userId);
            return Ok(new ApiResponse(200, "Tasks retrieved successfully", result));
        }

        // GET: api/DeveloperTasks/GetMyTaskById/5
        [HttpGet("GetMyTaskById/{id:int}")]
        public async Task<IActionResult> GetMyTask(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var task = await _taskService.GetMyTaskByIdAsync(id, userId);
            return Ok(new ApiResponse(200, "Task retrieved successfully", task));
        }

        // PUT: api/DeveloperTasks/UpdateMyTaskStatus/5
        [HttpPut("UpdateMyTaskStatus/{id:int}")]
        public async Task<IActionResult> UpdateMyTaskStatus(int id, UpdateTaskStatusDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _taskService.UpdateMyTaskStatusAsync(id, dto, userId);
            return Ok(new ApiResponse(200, "Task status updated successfully"));
        }

        // GET: api/DeveloperTasks/GetMyStatistics — احصائيات المطور (هو بس)
        [HttpGet("GetMyStatistics")]
        public async Task<IActionResult> GetMyStatistics()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _taskService.GetDeveloperStatisticsAsync(userId);
            return Ok(new ApiResponse(200, "Statistics retrieved successfully", result));
        }

        // GET: api/DeveloperTasks/GetMyControlPanel — لوحة تحكم المطور
        [HttpGet("GetMyControlPanel")]
        public async Task<IActionResult> GetMyControlPanel()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _taskService.GetDeveloperControlPanelAsync(userId);
            return Ok(new ApiResponse(200, "Control panel retrieved successfully", result));
        }
    }
}
