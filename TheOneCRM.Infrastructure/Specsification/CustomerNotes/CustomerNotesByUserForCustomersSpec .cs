using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CustomerNotes
{
    public class CustomerNotesByUserForCustomersSpec : BaseSpecification<CustomerNote>
    {
        public CustomerNotesByUserForCustomersSpec(IReadOnlyCollection<int> customerIds)
        : base(n => customerIds.Contains(n.CustomerId))
        //: base(n => customerIds.Contains(n.CustomerId) && n.CreatedById == userId)
        {
        }
    }
}
