using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class CampaignWithDetailsSpec : BaseSpecification<Campaigns>
    {
        public CampaignWithDetailsSpec()
        {
            AddInclude(x => x.ChannelSource);
            AddInclude(x => x.Customers);
        }

        public CampaignWithDetailsSpec(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ChannelSource);
            AddInclude(x => x.Customers);
        }
    }
}
