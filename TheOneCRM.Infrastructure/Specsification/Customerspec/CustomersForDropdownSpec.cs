using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // كل العملاء (لو assignedToId = null) أو عملاء مستخدم معيّن بس
    public class CustomersForDropdownSpec : BaseSpecification<Customer>
    {
        public CustomersForDropdownSpec(string? assignedToId)
            : base(c => assignedToId == null || c.AssignedToId == assignedToId)
        {
        }
    }
}
