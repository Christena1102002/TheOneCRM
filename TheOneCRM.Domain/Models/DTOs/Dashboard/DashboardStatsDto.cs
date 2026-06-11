using System.Collections.Generic;

namespace TheOneCRM.Domain.Models.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        // ===== إحصائيات التاسكات =====
        public int TotalTasks { get; set; }
        public int TodoTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int ReviewTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }

        // ===== الكروت =====
        // مهام مستحقة اليوم
        public int TasksDueToday { get; set; }
        public int HighPriorityTasksDueToday { get; set; }

        // المشاريع المتأخرة (محتاجة مراجعة)
        public int OverdueProjects { get; set; }

        // المشاريع المكتملة (هذا الربع)
        public int CompletedProjectsThisQuarter { get; set; }

        // المشاريع النشطة + اللي اتضافت الشهر ده
        public int ActiveProjects { get; set; }
        public int ActiveProjectsAddedThisMonth { get; set; }

        // ===== الشارتس =====
        // إنتاجية المطور (هذا الأسبوع)
        public List<DailyProductivityDto> WeeklyProductivity { get; set; } = new();

        // نظرة عامة على تقدم المشروعات
        public List<WeeklyProgressDto> ProjectsProgress { get; set; } = new();
    }

    public class DailyProductivityDto
    {
        public string Day { get; set; } = null!;   // Mon, Tue, ...
        public int CompletedTasks { get; set; }
    }

    public class WeeklyProgressDto
    {
        public string Week { get; set; } = null!;  // Week 1, Week 2, ...
        public int Completed { get; set; }
        public int InProgress { get; set; }
    }
}
