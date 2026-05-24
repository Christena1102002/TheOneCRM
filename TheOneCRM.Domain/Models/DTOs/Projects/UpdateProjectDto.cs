using System;
using System.Collections.Generic;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Projects
{
    public class UpdateProjectDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string? ProjectManagerId { get; set; }

        public StatusOfProject Status { get; set; }
        public PriorityStatus Priority { get; set; }

        public decimal Price { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // قائمة المهندسين بعد التعديل (تستبدل القائمة الحالية)
        public List<string> EngineerIds { get; set; } = new();
    }
}
