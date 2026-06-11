namespace TheOneCRM.Domain.Models.Entities
{
    public class GoalCompletion : BaseEntity
    {
        public int GoalId { get; set; }
        public Goal Goal { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int PointsEarned { get; set; }
        public decimal? RewardEarned { get; set; }
        public DateTime AchievedAt { get; set; }
    }
}
