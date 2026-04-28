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
        public int priority { get; set; }

        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser CreatedBy { get; set; }
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public AppUser AssignedTo { get; set; }

    }
}
