using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Goal : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public GoalType Type { get; set; }
        public TimePeriod Period { get; set; }

        public int TargetValue { get; set; }
        public int Points { get; set; }
        public decimal? FinancialReward { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // مطلوب فقط لو Type = Individual
        public string? AssignedToId { get; set; }
        public AppUser? AssignedTo { get; set; }

        // التقدم الحالي نحو الهدف
        public int CurrentProgress { get; set; } = 0;
        public bool IsAchieved { get; set; } = false;
        public DateTime? AchievedAt { get; set; }

        // اللي أنشأ الهدف (Admin)
        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }

        public ICollection<GoalCompletion> Completions { get; set; } = new List<GoalCompletion>();
    }
}
