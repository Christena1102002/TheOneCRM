using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class CampaignCountry :BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaigns Campaign { get; set; }
    }
}
