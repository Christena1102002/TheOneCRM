using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.SupportTicketSpec
{
    public class SupportTicketByIdSpec : BaseSpecification<SupportTickets>
    {
        public SupportTicketByIdSpec(int id) : base(t => t.Id == id)
        {
            // مفيش includes — ProjectTo هيتولّى الموضوع
        }
    }
}
