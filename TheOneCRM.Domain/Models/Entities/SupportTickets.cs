using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class SupportTickets:BaseEntity
    {
        public int TicketNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusOfTickets Status { get; set; }
        public PriorityStatus priority { get; set; }

        // تاريخ حل التذكرة (يتسجّل لما الحالة تبقى Resolved)
        public DateTime? ResolvedAt { get; set; }

        // تاريخ المتابعة القادمة (مطلوب لما الحالة Open)
        public DateTime? NextFollowUpDate { get; set; }

        // العميل
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        // الخدمة
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser CreatedBy { get; set; }
        //public string? AssignedToId { get; set; }

        //[ForeignKey("AssignedToId")]
        //public AppUser AssignedTo { get; set; }

    }
}
