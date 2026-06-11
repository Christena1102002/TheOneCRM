namespace TheOneCRM.Domain.Models.DTOs.GoalDtos
{
    public class GoalResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; }
        public string Period { get; set; }
        public int TargetValue { get; set; }
        public int Points { get; set; }
        public decimal? FinancialReward { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrentProgress { get; set; }
        public bool IsAchieved { get; set; }
        public DateTime? AchievedAt { get; set; }
        public string? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
