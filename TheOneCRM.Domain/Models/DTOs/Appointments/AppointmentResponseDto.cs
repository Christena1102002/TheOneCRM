using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Appointments
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public AppointmentType Type { get; set; }
        public string TypeNameAr { get; set; } = string.Empty;

        public AppointmentPriority Priority { get; set; }
        public string PriorityNameAr { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }
        public string StatusNameAr { get; set; } = string.Empty;

        public string? Location { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }

        public string AssignedToId { get; set; } = string.Empty;
        public string AssignedToUserName { get; set; } = string.Empty;

        public int? CustomerId { get; set; }
        public string? CustomerFullName { get; set; }

        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
