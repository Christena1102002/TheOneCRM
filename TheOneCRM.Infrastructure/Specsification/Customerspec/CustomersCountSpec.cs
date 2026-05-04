using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;
using TheOneCRM.Infrastructure.Specsification.Customerspec;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class CustomersCountSpec : BaseSpecification<Customer>
    {
        public CustomersCountSpec() : base()
        {
        }
        // public CustomersCountSpec(CustomerPaginationParams p)
        //: base(CustomerFilters.Build(p))
        // {
        // }
        public CustomersCountSpec(CustomerPaginationParams p)
     : base(CustomerFilters.Build(p))
        {
        }
    }
}
