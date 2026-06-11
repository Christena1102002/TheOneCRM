using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.GoalDtos
{
    public class CreateGoalDto
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GoalType Type { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TimePeriod Period { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TargetValue { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Points { get; set; }

        public decimal? FinancialReward { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // مطلوب فقط لو Type = Individual
        public string? AssignedToId { get; set; }
    }
}
