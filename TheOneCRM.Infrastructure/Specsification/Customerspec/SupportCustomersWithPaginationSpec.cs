using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;
using TheOneCRM.Infrastructure.Specsification.Customerspec;
namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class SupportCustomersWithPaginationSpec : BaseSpecification<Customer>
    {
        public SupportCustomersWithPaginationSpec(CustomerPaginationParams p, string? currentUserId, bool isSupportOnly)
            : base(SupportCustomerFilters.Build(p, currentUserId, isSupportOnly))
                  {
            AddInclude(c => c.campaigns.ChannelSource);
            AddInclude("AssignmentHistory.ToUser");
            AddInclude("customerServices.Service");
            AddInclude("Activities.CreatedBy");
            ApplyOrderByDescending(c => c.CreatedAt);
            ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);
        }


    }
    
}
