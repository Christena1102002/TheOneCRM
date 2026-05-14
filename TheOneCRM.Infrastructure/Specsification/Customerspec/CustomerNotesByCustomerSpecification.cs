using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class CustomerNotesByCustomerSpecification : BaseSpecification<CustomerNote>
    {
        public CustomerNotesByCustomerSpecification(int customerId, string? role = null)
            : base(x => x.CustomerId == customerId
                     && (role == null || x.Role == role))
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.CreatedBy);
            ApplyOrderByDescending(x => x.CreatedAt);
        }
    }
}
