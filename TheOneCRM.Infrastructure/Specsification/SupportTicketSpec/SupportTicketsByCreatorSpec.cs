using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.SupportTicketSpec
{
    // كل تذاكر موظف دعم معيّن (اللي أنشأها)
    public class SupportTicketsByCreatorSpec : BaseSpecification<SupportTickets>
    {
        public SupportTicketsByCreatorSpec(string userId)
            : base(t => t.CreatedById == userId)
        {
        }
    }
}
