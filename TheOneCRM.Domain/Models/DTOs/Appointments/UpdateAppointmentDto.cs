using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Appointments
{
    public class UpdateAppointmentDto
    {
        public string Title { get; set; } = string.Empty;

       
        public string? Description { get; set; }

        
        public DateTime StartDate { get; set; }

      
        public DateTime EndDate { get; set; }

      
        public AppointmentType Type { get; set; }

        public AppointmentPriority Priority { get; set; } = AppointmentPriority.Low;

      
        public string AssignedToUserId { get; set; } = string.Empty;

        public string? Location { get; set; }

        
        public string? MeetingLink { get; set; }

        public string? Notes { get; set; }

        public int? CustomerId { get; set; }
    }
}
