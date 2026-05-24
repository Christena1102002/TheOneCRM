using System;
using System.Collections.Generic;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Projects
{
    public class CreateProjectDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int CustomerId { get; set; }
        public string? ProjectManagerId { get; set; }

        public StatusOfProject Status { get; set; } = StatusOfProject.Planned;
        public PriorityStatus Priority { get; set; } = PriorityStatus.Medium;

        public decimal Price { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // المهندسون المختارون للمشروع
        public List<string> EngineerIds { get; set; } = new();
    }
}
