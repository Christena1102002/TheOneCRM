using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CampaignDto
{
    public class CampaignDashboardDto
    {
        public int TotalCampaigns { get; set; }
        public int ActiveCampaigns { get; set; }

        public decimal TotalBudget { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal TotalRemaining { get; set; }
    }
}
