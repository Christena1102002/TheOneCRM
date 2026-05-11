using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class ActiveCampaignsWithParamSpecification:BaseSpecification<Campaigns>
    {
        public ActiveCampaignsWithParamSpecification():base(C=>C.Status==CampaignStatus.Active &&C.StartDate<=DateTime.UtcNow &&C.EndDate>=DateTime.UtcNow)
        {

        }
    }
}
