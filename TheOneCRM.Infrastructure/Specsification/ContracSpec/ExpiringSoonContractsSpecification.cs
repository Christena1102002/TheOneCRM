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
    public class ExpiringSoonContractsSpecification : BaseSpecification<Contract>
    {
        public ExpiringSoonContractsSpecification(DateTime today, DateTime after30Days)
            : base(c =>
                c.Status == ContractStatus.Active &&
                c.EndDate.HasValue &&
                c.EndDate.Value.Date >= today &&
                c.EndDate.Value.Date <= after30Days)
        {
        }
    }
}
