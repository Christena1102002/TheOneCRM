using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class SalesCustomersCountSpec : BaseSpecification<Customer>
    {
        public SalesCustomersCountSpec(
            CustomerPaginationParams p,
            string? currentUserId,
            bool isSalesOnly)
            : base(SalesCustomerFilters.Build(p, currentUserId, isSalesOnly))
        {
        }
    }
}
