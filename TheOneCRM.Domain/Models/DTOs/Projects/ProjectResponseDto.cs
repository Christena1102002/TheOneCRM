using System;
using System.Collections.Generic;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Projects
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        // بيانات العميل
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }

        // مدير المشروع
        public string? ProjectManagerId { get; set; }
        public string? ProjectManagerName { get; set; }

        // اللي أنشأ المشروع
        public string? CreatedById { get; set; }
        public string? CreatedByName { get; set; }

        // الحالة والأولوية
        public StatusOfProject Status { get; set; }
        public string StatusName { get; set; } = null!;
        public PriorityStatus Priority { get; set; }
        public string PriorityName { get; set; } = null!;

        public decimal Price { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // نسبة التقدّم (محسوبة من المهام المكتملة) 0-100
        public int Progress { get; set; }

        // المهندسون وعدد أعضاء الفريق (محسوب من عددهم)
        public List<ProjectEngineerDto> Engineers { get; set; } = new();
        public int TeamMembersCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
