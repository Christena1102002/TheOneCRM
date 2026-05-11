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
    public class TodayCustomersSpecification:BaseSpecification<Customer>
    {
        public TodayCustomersSpecification()
       : base(c => c.CreatedAt >= DateTime.UtcNow.Date
                   && c.CreatedAt < DateTime.UtcNow.Date.AddDays(1)&&c.status ==StatusOfCustomers.New)
        {
        }
    }
}
