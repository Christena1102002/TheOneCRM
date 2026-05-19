using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Appointment : BaseEntity
    {
        // عنوان الموعد
        public string Title { get; set; } = null!;

        // وصف الموعد
        public string? Description { get; set; }

        // تاريخ البداية
        public DateTime StartDate { get; set; }

        // تاريخ النهاية
        public DateTime EndDate { get; set; }

        // نوع الموعد
        public AppointmentType Type { get; set; }

        // الأولوية
        public AppointmentPriority Priority { get; set; }

        // حالة الموعد
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        // المكان
        public string? Location { get; set; }

        // رابط الاجتماع الافتراضي
        public string? MeetingLink { get; set; }

        // ملاحظات
        public string? Notes { get; set; }

        // الشخص المسؤول عن الموعد
        public string AssignedToId { get; set; } = null!;
        public AppUser AssignedTo { get; set; } = null!;

        // من أنشأ الموعد
        public string? CreatedById { get; set; }
        public AppUser CreatedBy { get; set; } = null!;

        // ربط اختياري بعميل
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
