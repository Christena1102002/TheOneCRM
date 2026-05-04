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
    public class CustomersWithFilterSpec : BaseSpecification<Customer>
    {
        public class CustomersWithPaginationSpec : BaseSpecification<Customer>
        {
            //public CustomersWithPaginationSpec(CustomerPaginationParams p) : base()
            //{
            //    // البيانات المرتبطة
            //    AddInclude(c => c.campaigns.ChannelSource);
            //    AddInclude(c => c.AssignedTo);
                
            //    AddInclude("customerServices.Service");

            //    // الترتيب: الأحدث أولاً
            //    ApplyOrderByDescending(c => c.CreatedAt);

            //    // Pagination
            //    ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);
            //}
            public CustomersWithPaginationSpec(CustomerPaginationParams p)
    : base(CustomerFilters.Build(p))
            {
                AddInclude(c => c.campaigns.ChannelSource);
                AddInclude(c => c.AssignedTo);

                AddInclude("customerServices.Service");

                // الترتيب: الأحدث أولاً
                ApplyOrderByDescending(c => c.CreatedAt);

                // Pagination
                ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);
            }
        }
    }
}
