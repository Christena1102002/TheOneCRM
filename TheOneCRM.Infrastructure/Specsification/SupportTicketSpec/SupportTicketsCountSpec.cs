using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.SupportTickets;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.SupportTicketSpec
{
    public class SupportTicketsCountSpec : BaseSpecification<SupportTickets>
    {
        public SupportTicketsCountSpec(SupportTicketParams p, string userId, bool isAdmin)
            : base(SupportTicketFilter.Build(p, userId, isAdmin))
        {
        }
    }
}
