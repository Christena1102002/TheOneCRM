using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CampaignDto
{
    public class CampaignResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SourceName { get; set; }
    }
}
