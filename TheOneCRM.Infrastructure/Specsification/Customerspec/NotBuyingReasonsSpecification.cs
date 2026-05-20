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
    public class NotBuyingReasonsSpecification : BaseSpecification<Customer>
    {
        public NotBuyingReasonsSpecification()
            : base(c =>
                c.status == StatusOfCustomers.NotBuyer &&
                !string.IsNullOrWhiteSpace(c.NotBuyingReason))
        {
            // ترتيب البيانات من قاعدة البيانات
            ApplyOrderBy(c => c.NotBuyingReason);
        }
    }
}
