using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.CampaignDto
{
    public class UpdateCampaignDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ChannelSourceId { get; set; }
        public decimal? Budget { get; set; }
        public int? DurationDays { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CampaignStatus? Status { get; set; }
        public Gender? Gender { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public List<int>? CountryIds { get; set; }
    }
}
