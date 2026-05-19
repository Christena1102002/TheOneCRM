using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;
using TheOneCRM.Infrastructure.Specsification.Customerspec;


public class SupportCustomersCountSpec : BaseSpecification<Customer>
{
    public SupportCustomersCountSpec(
        CustomerPaginationParams p,
        string? currentUserId,
        bool isSalesOnly)
        : base(SalesCustomerFilters.Build(p, currentUserId, isSalesOnly))
    {
    }

}
   
