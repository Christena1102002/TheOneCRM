using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.SupportTicketSpec
{
    // بترجّع التذكرة صاحبة أكبر رقم عشان نولّد الرقم اللي بعده
    public class LastTicketNumberSpec : BaseSpecification<SupportTickets>
    {
        public LastTicketNumberSpec()
        {
            ApplyOrderByDescending(t => t.TicketNumber);
        }
    }
}
