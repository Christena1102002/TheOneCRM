using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class CampaignsWithCustomersSpec : BaseSpecification<Campaigns>
    {
        public CampaignsWithCustomersSpec() : base()
        {
            AddInclude(c => c.Customers);
        }
    }
}
