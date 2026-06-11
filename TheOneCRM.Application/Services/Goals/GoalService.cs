using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.IGoals;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.GoalDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.GoalSpec;

namespace TheOneCRM.Application.Services.Goals
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GoalResponseDto> CreateGoalAsync(CreateGoalDto dto, string createdById)
        {
            if (dto.EndDate <= dto.StartDate)
                throw new InvalidOperationException("End date must be after start date.");

            if (dto.Type == GoalType.Individual)
            {
                if (string.IsNullOrWhiteSpace(dto.AssignedToId))
                    throw new InvalidOperationException("AssignedToId is required for individual goals.");

                var user = await _userManager.FindByIdAsync(dto.AssignedToId);
                if (user == null)
                    throw new KeyNotFoundException("Assigned user not found.");

                var isSales = await _userManager.IsInRoleAsync(user, "Sales");
                if (!isSales)
                    throw new InvalidOperationException("Goal can only be assigned to a Sales person.");
            }

            var goal = _mapper.Map<Goal>(dto);
            goal.CreatedById = createdById;

            await _unitOfWork.Repository<Goal>().AddAsync(goal);
            await _unitOfWork.SaveChangesAsync();

            return await GetGoalByIdAsync(goal.Id);
        }

        public async Task<GoalResponseDto> GetGoalByIdAsync(int id)
        {
            var goal = await _unitOfWork.Repository<Goal>()
                .GetEntityWithSpec(new GoalWithDetailsSpec(id));
            if (goal == null)
                throw new KeyNotFoundException($"Goal {id} not found.");
            return MapToDto(goal);
        }

        public async Task<List<GoalResponseDto>> GetAllGoalsAsync(GoalFilterParams filter)
        {
            var goals = await _unitOfWork.Repository<Goal>()
                .ListAsync(new GoalWithDetailsSpec(filter));
            return goals.Select(MapToDto).ToList();
        }

        public async Task<List<GoalResponseDto>> GetGoalsByUserAsync(string userId, GoalFilterParams filter)
        {
            var goals = await _unitOfWork.Repository<Goal>()
                .ListAsync(new GoalWithDetailsSpec(userId, filter));
            return goals.Select(MapToDto).ToList();
        }

        public async Task<GoalResponseDto> UpdateGoalAsync(int id, UpdateGoalDto dto)
        {
            var goal = await _unitOfWork.Repository<Goal>().GetByIdAsync(id);
            if (goal == null)
                throw new KeyNotFoundException($"Goal {id} not found.");

            if (dto.EndDate <= dto.StartDate)
                throw new InvalidOperationException("End date must be after start date.");

            goal.Title = dto.Title;
            goal.Description = dto.Description;
            goal.Period = dto.Period;
            goal.TargetValue = dto.TargetValue;
            goal.Points = dto.Points;
            goal.FinancialReward = dto.FinancialReward;
            goal.StartDate = dto.StartDate;
            goal.EndDate = dto.EndDate;
            goal.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Goal>().Update(goal);
            await _unitOfWork.SaveChangesAsync();

            return await GetGoalByIdAsync(id);
        }

        public async Task<GoalResponseDto> UpdateGoalProgressAsync(int id, UpdateGoalProgressDto dto)
        {
            var goal = await _unitOfWork.Repository<Goal>()
                .GetEntityWithSpec(new GoalWithDetailsSpec(id));
            if (goal == null)
                throw new KeyNotFoundException($"Goal {id} not found.");

            if (goal.IsAchieved)
                throw new InvalidOperationException("Goal is already achieved.");

            goal.CurrentProgress += dto.Value;
            goal.UpdatedAt = DateTime.UtcNow;

            // لو وصل أو تخطى الهدف → تحقق الهدف
            if (goal.CurrentProgress >= goal.TargetValue)
            {
                goal.CurrentProgress = goal.TargetValue;
                goal.IsAchieved = true;
                goal.AchievedAt = DateTime.UtcNow;

                await AwardPointsAsync(goal);
            }

            _unitOfWork.Repository<Goal>().Update(goal);
            await _unitOfWork.SaveChangesAsync();

            return await GetGoalByIdAsync(id);
        }

        public async Task<GoalStatsDto> GetGoalStatsAsync(string? userId = null)
        {
            GoalFilterParams emptyFilter = new();
            List<Goal> goals;

            if (userId == null)
                goals = (await _unitOfWork.Repository<Goal>().ListAsync(new GoalWithDetailsSpec(emptyFilter))).ToList();
            else
                goals = (await _unitOfWork.Repository<Goal>().ListAsync(new GoalWithDetailsSpec(userId, emptyFilter))).ToList();

            // تجميع الـ completions
            IEnumerable<GoalCompletion> completions;
            if (userId == null)
                completions = goals.SelectMany(g => g.Completions);
            else
                completions = goals.SelectMany(g => g.Completions).Where(c => c.UserId == userId);

            var completionsList = completions.ToList();

            return new GoalStatsDto
            {
                TotalGoals = goals.Count,
                IndividualGoals = goals.Count(g => g.Type == GoalType.Individual),
                TeamGoals = goals.Count(g => g.Type == GoalType.Team),
                AchievedGoals = goals.Count(g => g.IsAchieved),
                InProgressGoals = goals.Count(g => !g.IsAchieved),
                TotalPointsAwarded = completionsList.Sum(c => c.PointsEarned),
                PointsBreakdown = completionsList.Select(c => new GoalPointDetailDto
                {
                    GoalId = c.GoalId,
                    GoalTitle = c.Goal?.Title ?? "",
                    GoalType = c.Goal?.Type.ToString() ?? "",
                    Period = c.Goal?.Period.ToString() ?? "",
                    StartDate = c.Goal?.StartDate ?? default,
                    EndDate = c.Goal?.EndDate ?? default,
                    PointsEarned = c.PointsEarned,
                    RewardEarned = c.RewardEarned,
                    AchievedAt = c.AchievedAt,
                    UserName = userId == null ? c.User?.FullName : null
                }).OrderByDescending(x => x.AchievedAt).ToList()
            };
        }

        public async Task DeleteGoalAsync(int id)
        {
            var goal = await _unitOfWork.Repository<Goal>().GetByIdAsync(id);
            if (goal == null)
                throw new KeyNotFoundException($"Goal {id} not found.");

            _unitOfWork.Repository<Goal>().Delete(goal);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task AwardPointsAsync(Goal goal)
        {
            if (goal.Type == GoalType.Individual && !string.IsNullOrEmpty(goal.AssignedToId))
            {
                await _unitOfWork.Repository<GoalCompletion>().AddAsync(new GoalCompletion
                {
                    GoalId = goal.Id,
                    UserId = goal.AssignedToId,
                    PointsEarned = goal.Points,
                    RewardEarned = goal.FinancialReward,
                    AchievedAt = DateTime.UtcNow
                });
            }
            else if (goal.Type == GoalType.Team)
            {
                var salesUsers = await _userManager.GetUsersInRoleAsync("Sales");
                foreach (var user in salesUsers)
                {
                    await _unitOfWork.Repository<GoalCompletion>().AddAsync(new GoalCompletion
                    {
                        GoalId = goal.Id,
                        UserId = user.Id,
                        PointsEarned = goal.Points,
                        RewardEarned = goal.FinancialReward,
                        AchievedAt = DateTime.UtcNow
                    });
                }
            }
        }

        private GoalResponseDto MapToDto(Goal goal) => new GoalResponseDto
        {
            Id = goal.Id,
            Title = goal.Title,
            Description = goal.Description,
            Type = goal.Type.ToString(),
            Period = goal.Period.ToString(),
            TargetValue = goal.TargetValue,
            Points = goal.Points,
            FinancialReward = goal.FinancialReward,
            StartDate = goal.StartDate,
            EndDate = goal.EndDate,
            CurrentProgress = goal.CurrentProgress,
            IsAchieved = goal.IsAchieved,
            AchievedAt = goal.AchievedAt,
            AssignedToId = goal.AssignedToId,
            AssignedToName = goal.AssignedTo?.FullName,
            CreatedByName = goal.CreatedBy?.FullName,
            CreatedAt = goal.CreatedAt
        };
    }
}
