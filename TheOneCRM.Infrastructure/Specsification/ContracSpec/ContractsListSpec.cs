using System;
using System.Collections.Generic;
using TheOneCRM.Domain.Models.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Contracts;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ContracSpec
{
    public class ContractsListSpec : BaseSpecification<Contract>
    {
        public ContractsListSpec(ContractParams p, string? ownerId) : base(x =>
            (ownerId == null || x.CreatedById == ownerId) &&
            (string.IsNullOrEmpty(p.Search) ||
                x.Title.Contains(p.Search) ||
                x.Customer.FullName.Contains(p.Search) ||
                (x.Customer.CampanyName != null && x.Customer.CampanyName.Contains(p.Search)) ||
                (x.Customer.Phone != null && x.Customer.Phone.Contains(p.Search)))
        )
        {
            ApplyThenByDescending(x => x.CreatedAt);
            ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }

}
