using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class CustomersByCampaignSpec : BaseSpecification<Customer>
    {
        public CustomersByCampaignSpec(int campaignId)
            : base(x => x.compaignId == campaignId)
        {
        }

        // كل العملاء (للإجماليات العامة)
        public CustomersByCampaignSpec() : base() { }
    }
}
