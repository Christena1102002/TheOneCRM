using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class CampaignByIdSpec : BaseSpecification<Campaigns>
    {
        public CampaignByIdSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(c => c.ChannelSource);
             AddInclude(c => c.Countries);
            AddInclude(c=>c.appUser);
        }
    }
}
