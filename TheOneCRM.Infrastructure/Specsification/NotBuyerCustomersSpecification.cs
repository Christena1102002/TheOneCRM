using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class NotBuyerCustomersSpecification : BaseSpecification<Customer>
    {
        public NotBuyerCustomersSpecification()
           : base(c => c.status == StatusOfCustomers.NotBuyer)
        {
        }
    }
}
