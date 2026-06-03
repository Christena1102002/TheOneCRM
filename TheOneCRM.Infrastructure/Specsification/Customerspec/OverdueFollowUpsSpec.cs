using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class OverdueFollowUpsSpec : BaseSpecification<Customer>
    {
        public OverdueFollowUpsSpec(string? salesPersonId, DateTime today)
            : base(c => (salesPersonId == null || c.AssignedToId == salesPersonId)
                     && c.NextFollowUpDate.HasValue
                     && c.NextFollowUpDate.Value < today)
        {
        }
    }
}
