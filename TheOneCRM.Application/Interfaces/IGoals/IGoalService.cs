using TheOneCRM.Domain.Models.DTOs.GoalDtos;

namespace TheOneCRM.Application.Interfaces.IGoals
{
    public interface IGoalService
    {
        Task<GoalResponseDto> CreateGoalAsync(CreateGoalDto dto, string createdById);
        Task<GoalResponseDto> GetGoalByIdAsync(int id);
        Task<List<GoalResponseDto>> GetAllGoalsAsync(GoalFilterParams filter);
        Task<List<GoalResponseDto>> GetGoalsByUserAsync(string userId, GoalFilterParams filter);
        Task<GoalResponseDto> UpdateGoalAsync(int id, UpdateGoalDto dto);
        Task<GoalResponseDto> UpdateGoalProgressAsync(int id, UpdateGoalProgressDto dto);
        Task<GoalStatsDto> GetGoalStatsAsync(string? userId = null);
        Task DeleteGoalAsync(int id);
    }
}
