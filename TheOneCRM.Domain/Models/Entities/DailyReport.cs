using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class DailyReport :BaseEntity
    {
        public DateTime ReportDate { get; set; }
        // المهام المنجزة - مطلوب
        public string CompletedTasks { get; set; } = string.Empty;
        // المهام قيد التنفيذ
        public string? TasksInProgress { get; set; }

        // المهام المخططة للأيام القادمة
        public string? PlannedTasks { get; set; }

        // التحديات والمشاكل
        public string? Challenges { get; set; }
        public decimal WorkHours { get; set; }

        // ملاحظات إضافية
        public string? AdditionalNotes { get; set; }
        // FK: الموظف صاحب التقرير
        public string UserId { get; set; } = string.Empty;
        public AppUser appUser { get; set; } = null!;
    }
}
