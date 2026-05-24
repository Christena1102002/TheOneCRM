using System;
using System.Collections.Generic;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Dashboard
{
    public class ControlPanelDto
    {
        // ===== الكروت =====
        public int DeadlinesToday { get; set; }  // مواعيد نهائية اليوم
        public int CompletedTasks { get; set; }  // المهام المنجزة
        public int AssignedTasks { get; set; }   // المهام المعينة (غير المكتملة)
        public int ActiveProjects { get; set; }  // المشاريع النشطة

        // المواعيد النهائية اليوم
        public List<DeadlineItemDto> TodayDeadlines { get; set; } = new();

        // أداء السبرنت
        public List<SprintPerformanceDto> SprintPerformance { get; set; } = new();
    }

    public class DeadlineItemDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? ProjectName { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityStatus Priority { get; set; }
    }

    public class SprintPerformanceDto
    {
        public string Sprint { get; set; } = null!;  // Sprint 1, Sprint 2, ...
        public int Planned { get; set; }             // المخطط
        public int Completed { get; set; }           // المنجز
    }
}
