using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;
using TheOneCRM.Domain.Models.Entities;
namespace TheOneCRM.Infrastructure.Specsification.ContracSpec
{
    public class ExpiredContractsCountSpecification : BaseSpecification<Contract>
    {
        public ExpiredContractsCountSpecification()
            : base(c => c.Status == ContractStatus.Expired)
        {
        }
    }
}
