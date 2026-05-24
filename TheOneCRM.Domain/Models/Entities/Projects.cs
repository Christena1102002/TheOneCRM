using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Projects : BaseEntity
    {
        // اسم المشروع
        public string Title { get; set; } = null!;

        // وصف المشروع
        public string? Description { get; set; }

        // حالة المشروع (مخطط افتراضيًا)
        public StatusOfProject Status { get; set; } = StatusOfProject.Planned;

        // أولوية المشروع
        public PriorityStatus Priority { get; set; } = PriorityStatus.Medium;

        // الميزانية (ريال سعودي)
        public decimal Price { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // العميل
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        // الأدمن اللي أنشأ المشروع
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser? CreatedBy { get; set; }

        // مدير المشروع
        public string? ProjectManagerId { get; set; }

        [ForeignKey("ProjectManagerId")]
        public AppUser? ProjectManager { get; set; }

        // المهندسون المعيّنون للمشروع (many-to-many)
        public ICollection<ProjectEngineer> ProjectEngineers { get; set; }
            = new List<ProjectEngineer>();

        // مهام المشروع (نحسب منها نسبة التقدّم)
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
