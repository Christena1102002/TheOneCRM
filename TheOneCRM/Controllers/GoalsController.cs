using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.IGoals;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.GoalDtos;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalsController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (dto.Type == GoalType.Individual && string.IsNullOrWhiteSpace(dto.AssignedToId))
                return BadRequest(new ApiResponse(400, "AssignedToId is required for individual goals."));

            var result = await _goalService.CreateGoalAsync(dto, userId);
            return StatusCode(201, new ApiResponse(201, "Goal created successfully", result));
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetAllGoals([FromQuery] GoalFilterParams filter)
        {
            var result = await _goalService.GetAllGoalsAsync(filter);
            return Ok(new ApiResponse(200, "Goals retrieved successfully", result));
        }

        [HttpGet("my")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales}")]
        public async Task<IActionResult> GetMyGoals([FromQuery] GoalFilterParams filter)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _goalService.GetGoalsByUserAsync(userId, filter);
            return Ok(new ApiResponse(200, "Goals retrieved successfully", result));
        }

        [HttpGet("stats")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales}")]
        public async Task<IActionResult> GetStats()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // الأدمن يشوف كل الإحصائيات، السيلز يشوف إحصائياته بس
            var isAdmin = User.IsInRole(UserRoles.Admin);
            var result = await _goalService.GetGoalStatsAsync(isAdmin ? null : userId);
            return Ok(new ApiResponse(200, "Stats retrieved successfully", result));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales}")]
        public async Task<IActionResult> GetGoalById(int id)
        {
            var result = await _goalService.GetGoalByIdAsync(id);
            return Ok(new ApiResponse(200, "Goal retrieved successfully", result));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] UpdateGoalDto dto)
        {
            var result = await _goalService.UpdateGoalAsync(id, dto);
            return Ok(new ApiResponse(200, "Goal updated successfully", result));
        }

        [HttpPatch("{id}/progress")]
        [Authorize(Roles = UserRoles.Sales)]
        public async Task<IActionResult> UpdateGoalProgress(int id, [FromBody] UpdateGoalProgressDto dto)
        {
            var result = await _goalService.UpdateGoalProgressAsync(id, dto);
            return Ok(new ApiResponse(200, "Goal progress updated successfully", result));
        }

        [HttpGet("{id}/stats")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales}")]
        public async Task<IActionResult> GetGoalStats(int id)
        {
            var result = await _goalService.GetGoalByIdAsync(id);
            return Ok(new ApiResponse(200, "Goal retrieved successfully", result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            await _goalService.DeleteGoalAsync(id);
            return Ok(new ApiResponse(200, "Goal deleted successfully"));
        }
    }
}
