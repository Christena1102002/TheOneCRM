using TheOneCRM.Domain.Models.DTOs.GoalDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.GoalSpec
{
    public class GoalWithDetailsSpec : BaseSpecification<Goal>
    {
        // جيب هدف واحد بكل تفاصيله
        public GoalWithDetailsSpec(int id)
            : base(g => g.Id == id)
        {
            AddInclude(g => g.AssignedTo);
            AddInclude(g => g.CreatedBy);
            AddInclude("Completions.User");
        }

        // جيب كل الأهداف مع فلترة اختيارية
        public GoalWithDetailsSpec(GoalFilterParams filter)
            : base(g =>
                (string.IsNullOrEmpty(filter.Search) || g.Title.Contains(filter.Search)) &&
                (!filter.Type.HasValue || g.Type == filter.Type.Value) &&
                (!filter.Period.HasValue || g.Period == filter.Period.Value) &&
                (!filter.IsAchieved.HasValue || g.IsAchieved == filter.IsAchieved.Value))
        {
            AddInclude(g => g.AssignedTo);
            AddInclude(g => g.CreatedBy);
            AddInclude("Completions.User");
            ApplyOrderByDescending(g => g.CreatedAt);
        }

        // أهداف مستخدم معين (فردية بتاعته + جماعية) مع فلترة
        public GoalWithDetailsSpec(string userId, GoalFilterParams filter)
            : base(g =>
                (g.Type == GoalType.Team || (g.Type == GoalType.Individual && g.AssignedToId == userId)) &&
                (string.IsNullOrEmpty(filter.Search) || g.Title.Contains(filter.Search)) &&
                (!filter.Type.HasValue || g.Type == filter.Type.Value) &&
                (!filter.Period.HasValue || g.Period == filter.Period.Value) &&
                (!filter.IsAchieved.HasValue || g.IsAchieved == filter.IsAchieved.Value))
        {
            AddInclude(g => g.AssignedTo);
            AddInclude(g => g.CreatedBy);
            AddInclude("Completions.User");
            ApplyOrderByDescending(g => g.CreatedAt);
        }
    }
}
