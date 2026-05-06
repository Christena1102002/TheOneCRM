using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class CampaignsSpec : BaseSpecification<Campaigns>
    {
        public CampaignsSpec()
        {
            AddInclude(c => c.ChannelSource);
            AddInclude(c => c.appUser);
        }
    }
}
