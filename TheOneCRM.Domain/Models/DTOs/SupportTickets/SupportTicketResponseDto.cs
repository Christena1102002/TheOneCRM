using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.SupportTickets
{
    public class SupportTicketResponseDto
    {
        public int Id { get; set; }
        public int TicketNumber { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        // العميل
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }

        // الخدمة
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }

        // الأولوية
        public PriorityStatus Priority { get; set; }
        public string PriorityName { get; set; } = null!;

        // الحالة
        public StatusOfTickets Status { get; set; }
        public string StatusName { get; set; } = null!;


        // اللي أنشأ التذكرة
        public string? CreatedById { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
    }
}
