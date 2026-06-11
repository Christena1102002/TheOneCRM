namespace TheOneCRM.Domain.Models.DTOs.GoalDtos
{
    public class GoalStatsDto
    {
        public int TotalGoals { get; set; }
        public int IndividualGoals { get; set; }
        public int TeamGoals { get; set; }
        public int AchievedGoals { get; set; }
        public int InProgressGoals { get; set; }
        public int TotalPointsAwarded { get; set; }
        public List<GoalPointDetailDto> PointsBreakdown { get; set; } = new();
    }

    public class GoalPointDetailDto
    {
        public int GoalId { get; set; }
        public string GoalTitle { get; set; }
        public string GoalType { get; set; }
        public string Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PointsEarned { get; set; }
        public decimal? RewardEarned { get; set; }
        public DateTime AchievedAt { get; set; }
        // للأدمن فقط: اسم الشخص اللي حقق الهدف
        public string? UserName { get; set; }
    }
}
