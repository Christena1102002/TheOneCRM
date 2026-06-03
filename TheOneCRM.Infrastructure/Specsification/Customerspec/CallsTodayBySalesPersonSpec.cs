using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class CallsTodayBySalesPersonSpec : BaseSpecification<Customer>
    {
        public CallsTodayBySalesPersonSpec(string? salesPersonId, DateTime today, DateTime tomorrow)
            : base(c => (salesPersonId == null || c.AssignedToId == salesPersonId)
                     && c.LastFollowUpDate.HasValue
                     && c.LastFollowUpDate.Value >= today
                     && c.LastFollowUpDate.Value < tomorrow
                     && c.status == StatusOfCustomers.Contacted
            )
        {
        }
    }
}
