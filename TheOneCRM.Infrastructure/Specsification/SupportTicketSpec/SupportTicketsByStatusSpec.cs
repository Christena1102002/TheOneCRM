using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.SupportTicketSpec
{
    public class SupportTicketsByStatusSpec : BaseSpecification<SupportTickets>
    {
        public SupportTicketsByStatusSpec(StatusOfTickets status)
            : base(t => t.Status == status)
        {
        }
    }
}
