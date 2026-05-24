using System;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Tasks
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        // المشروع
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }

        // المطور المعيّن
        public string? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }

        // المدير اللي أنشأ
        public string? CreatedById { get; set; }
        public string? CreatedByName { get; set; }

        public StatusOfTask Status { get; set; }
        public string StatusName { get; set; } = null!;
        public PriorityStatus Priority { get; set; }
        public string PriorityName { get; set; } = null!;
        public TaskCategory Category { get; set; }
        public string CategoryName { get; set; } = null!;

        public DateTime DueDate { get; set; }
        public int? EstimatedHours { get; set; }
        public int? ActualHours { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Tags { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
