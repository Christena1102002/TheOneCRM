using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // العملاء المعيّنين لمستخدم معيّن، أو اللي تم استشارتهم منه قبل كده (حتى لو رجعوا للمبيعات)
    public class AssignedCustomersByUserSpec : BaseSpecification<Customer>
    {
        public AssignedCustomersByUserSpec(string userId)
            : base(c => c.AssignedToId == userId
                     || c.AssignmentHistory.Any(h => h.ToUserId == userId && h.ToRole == "Support"))
        {
        }
    }
}
