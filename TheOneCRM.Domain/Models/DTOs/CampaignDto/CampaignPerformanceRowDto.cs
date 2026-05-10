using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CampaignDto
{
    public class CampaignPerformanceRowDto
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; } = string.Empty;
        public int TotalCustomers { get; set; }
        public int Buyers { get; set; }
        public int NonBuyers { get; set; }
        public decimal ConversionRate { get; set; } 

    }
    public class CampaignPerformanceResponseDto
    {
        public List<CampaignPerformanceRowDto> Rows { get; set; } = new();

        
        public CampaignPerformanceRowDto Total { get; set; } = new();
    }
}
