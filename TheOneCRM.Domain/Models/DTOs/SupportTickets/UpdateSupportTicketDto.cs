using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.SupportTickets
{
    public class UpdateSupportTicketDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int ServiceId { get; set; }

        public PriorityStatus Priority { get; set; }
        public StatusOfTickets Status { get; set; }

        // مطلوب لما الحالة Open
        public DateTime? NextFollowUpDate { get; set; }
    }
}
