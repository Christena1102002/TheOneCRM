using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class CustomerNoteByUserAndCustomerSpec : BaseSpecification<CustomerNote>
    {
        public CustomerNoteByUserAndCustomerSpec(int customerId, string userId)
            : base(x => x.CustomerId == customerId && x.CreatedById == userId)
        {
        }
    }
}
