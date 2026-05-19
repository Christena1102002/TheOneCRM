using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        public string Title { get; set; } = string.Empty;

        //[StringLength(2000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "نوع الموعد مطلوب")]
        public AppointmentType Type { get; set; } = AppointmentType.Meeting;

        public AppointmentPriority Priority { get; set; } = AppointmentPriority.Low;

        [Required(ErrorMessage = "لازم تختار الموظف المعين له الموعد")]
        public string AssignedToUserId { get; set; } = string.Empty;

        //[StringLength(500)]
        public string? Location { get; set; }

        //[StringLength(500)]
        public string? MeetingLink { get; set; }

        // ملاحظات
        public string? Notes { get; set; }

        public int? CustomerId { get; set; }
    }
}
