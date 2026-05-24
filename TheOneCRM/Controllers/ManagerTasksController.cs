using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.ITasks;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Tasks;

namespace TheOneCRM.API.Controllers
{
    // إدارة المهام — للأدمن (المدير) فقط: ينشئ ويشوف كل المهام
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class ManagerTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public ManagerTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // POST: api/ManagerTasks/CreateTask
        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var id = await _taskService.CreateTaskAsync(dto, userId);
            return Ok(new ApiResponse(200, "Task created successfully", new { id }));
        }

        // GET: api/ManagerTasks/GetTaskById/5
        [HttpGet("GetTaskById/{id:int}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(new ApiResponse(200, "Task retrieved successfully", task));
        }

        // GET: api/ManagerTasks/GetTasks — كل المهام لكل الناس
        [HttpGet("GetTasks")]
        public async Task<IActionResult> GetTasks([FromQuery] TaskParams p)
        {
            var result = await _taskService.GetTasksAsync(p);
            return Ok(new ApiResponse(200, "Tasks retrieved successfully", result));
        }

        // PUT: api/ManagerTasks/UpdateTask/5
        [HttpPut("UpdateTask/{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            await _taskService.UpdateTaskAsync(id, dto);
            return Ok(new ApiResponse(200, "Task updated successfully"));
        }

        // DELETE: api/ManagerTasks/DeleteTask/5
        [HttpDelete("DeleteTask/{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return Ok(new ApiResponse(200, "Task deleted successfully"));
        }

        // GET: api/ManagerTasks/GetStatistics — احصائيات كل المطورين (للأدمن)
        [HttpGet("GetStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _taskService.GetAdminStatisticsAsync();
            return Ok(new ApiResponse(200, "Statistics retrieved successfully", result));
        }

        // GET: api/ManagerTasks/GetControlPanel — لوحة تحكم الأدمن (كل البيانات)
        [HttpGet("GetControlPanel")]
        public async Task<IActionResult> GetControlPanel()
        {
            var result = await _taskService.GetAdminControlPanelAsync();
            return Ok(new ApiResponse(200, "Control panel retrieved successfully", result));
        }
    }
}
