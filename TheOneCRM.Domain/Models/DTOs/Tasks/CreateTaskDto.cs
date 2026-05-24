using System;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int ProjectId { get; set; }
        public string AssignedToId { get; set; } = null!;

        public StatusOfTask Status { get; set; } = StatusOfTask.ToDo;
        public PriorityStatus Priority { get; set; } = PriorityStatus.Medium;
        public TaskCategory Category { get; set; } = TaskCategory.Development;

        public DateTime DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public string? Tags { get; set; }
    }
}
