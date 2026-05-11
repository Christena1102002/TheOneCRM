using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class PotentialCustomersLast7DaysSpecification :BaseSpecification<Customer>
    {
        public PotentialCustomersLast7DaysSpecification()
            : base(c => c.CreatedAt >= DateTime.UtcNow.AddDays(-7) && c.CreatedAt < DateTime.UtcNow)
        {
        }
    }
}
