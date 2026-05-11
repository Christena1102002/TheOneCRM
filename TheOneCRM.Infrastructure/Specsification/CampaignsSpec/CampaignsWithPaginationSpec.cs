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
    public class CampaignsWithPaginationSpec : BaseSpecification<Campaigns>
    {
        public CampaignsWithPaginationSpec(CampaignPaginationParams p)
            : base(c =>
                (!p.ChannelSourceId.HasValue || c.ChannelSourceId == p.ChannelSourceId.Value) &&
                //(string.IsNullOrEmpty(p.Status) || c.Status.ToString() == p.Status) &&
               (!p.Status.HasValue || (int)c.Status == p.Status.Value) &&   
            (string.IsNullOrEmpty(p.Search) || c.Name.ToLower().Contains(p.Search))
            )
        {
            AddInclude(c => c.ChannelSource);
            AddInclude(c=>c.Countries);
            AddInclude(c=>c.Customers);
            //if(!string.IsNullOrEmpty(p.Status))
            //{
            //    Enum.TryParse<CampaignStatus>(p.Status,true,out var statusEnum)
            //        {
            //        Criteria(c => c.Status == statusEnum);
            //    }
            //}

            ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);
        }
    }
}// عشان نجيب اسم الدولة
