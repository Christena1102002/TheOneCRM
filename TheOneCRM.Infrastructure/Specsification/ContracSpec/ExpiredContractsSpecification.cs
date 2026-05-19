using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;
using TheOneCRM.Domain.Models.Enums;
namespace TheOneCRM.Infrastructure.Specsification.ContracSpec
{
    public class ExpiredContractsSpecification : BaseSpecification<Contract>
    {
        public ExpiredContractsSpecification(DateTime today)
            : base(c =>
                c.Status != ContractStatus.Cancelled &&
                c.Status != ContractStatus.Expired &&
                c.EndDate.HasValue &&
                c.EndDate.Value.Date < today)
        {
        }
    }
}
