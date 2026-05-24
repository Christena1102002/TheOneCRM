using System;
using System.ComponentModel.DataAnnotations.Schema;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Tasks : BaseEntity
    {
        // عنوان المهمة
        public string Title { get; set; } = null!;

        // وصف المهمة
        public string? Description { get; set; }

        // الحالة (للإنجاز افتراضيًا)
        public StatusOfTask Status { get; set; } = StatusOfTask.ToDo;

        // الأولوية
        public PriorityStatus Priority { get; set; } = PriorityStatus.Medium;

        // الفئة
        public TaskCategory Category { get; set; } = TaskCategory.Development;

        // الموعد النهائي
        public DateTime DueDate { get; set; }

        // الوقت المقدّر (ساعات)
        public int? EstimatedHours { get; set; }

        // الساعات الفعلية اللي اتصرفت (يسجّلها المطوّر عند الإنهاء)
        public int? ActualHours { get; set; }

        // تاريخ إكمال المهمة فعليًا — يتظبط أوتوماتيك لما الحالة تبقى Completed
        public DateTime? CompletedAt { get; set; }

        // الوسوم (مفصولة بفاصلة)
        public string? Tags { get; set; }

        public int? OrderIndex { get; set; }

        // المشروع
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Projects? Project { get; set; }

        // المدير اللي أنشأ المهمة
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser? CreatedBy { get; set; }

        // المطور المعيّن إليه المهمة
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public AppUser? AssignedTo { get; set; }
    }
}
