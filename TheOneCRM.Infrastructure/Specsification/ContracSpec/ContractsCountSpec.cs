using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Contracts;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ContracSpec
{
    public class ContractsCountSpec : BaseSpecification<Contract>
    {
        public ContractsCountSpec(ContractParams p) : base(x =>
            (string.IsNullOrEmpty(p.Search) ||
                x.Title.Contains(p.Search) ||
                x.Customer.FullName.Contains(p.Search) ||
                (x.Customer.CampanyName != null && x.Customer.CampanyName.Contains(p.Search))) ||
        (x.Customer.Phone!=null && x.Customer.Phone.Contains(p.Search))
            //(!p.CustomerId.HasValue || x.CustomerId == p.CustomerId.Value)
        //&&
        //    (!p.Status.HasValue || x.Status == p.Status.Value)
        )
        { }
    }
}
