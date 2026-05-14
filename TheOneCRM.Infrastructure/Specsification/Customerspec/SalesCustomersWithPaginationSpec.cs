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
    public class SalesCustomersWithPaginationSpec : BaseSpecification<Customer>
    {
        public SalesCustomersWithPaginationSpec(
        CustomerPaginationParams p,
        string? currentUserId,
        bool isSalesOnly)
        : base(SalesCustomerFilters.Build(p, currentUserId, isSalesOnly))
        {
            AddInclude(c => c.campaigns.ChannelSource);
            AddInclude(c => c.AssignedTo);
            AddInclude("customerServices.Service");

            ApplyOrderByDescending(c => c.CreatedAt);
            ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);
        }
    }
}
