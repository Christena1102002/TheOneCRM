using System.ComponentModel.DataAnnotations;

namespace TheOneCRM.Domain.Models.DTOs.GoalDtos
{
    public class UpdateGoalProgressDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1.")]
        public int Value { get; set; }
    }
}
