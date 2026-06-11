using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.GoalDtos
{
    public class GoalFilterParams
    {
        public string? Search { get; set; }
        public GoalType? Type { get; set; }
        public TimePeriod? Period { get; set; }
        public bool? IsAchieved { get; set; }
    }
}
