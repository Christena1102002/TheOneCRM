using System;
using System.ComponentModel.DataAnnotations.Schema;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class TaskAssignee
    {
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Tasks Task { get; set; } = null!;

        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public AppUser User { get; set; } = null!;

        // حالة التاسك خاصة بكل مطور لوحده
        public StatusOfTask Status { get; set; } = StatusOfTask.ToDo;
        public int? ActualHours { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
