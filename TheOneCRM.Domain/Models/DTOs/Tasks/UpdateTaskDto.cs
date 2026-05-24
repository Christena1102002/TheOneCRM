using System;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string AssignedToId { get; set; } = null!;

        public StatusOfTask Status { get; set; }
        public PriorityStatus Priority { get; set; }
        public TaskCategory Category { get; set; }

        public DateTime DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public string? Tags { get; set; }
    }
}
