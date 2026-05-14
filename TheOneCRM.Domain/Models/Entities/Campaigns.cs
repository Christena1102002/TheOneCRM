using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Campaigns :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ChannelSourceId { get; set; }
        public ChannelSource ChannelSource { get; set; }
        public CampaignStatus Status { get; set; } = CampaignStatus.Active;
        public decimal Budget { get; set; }
        public decimal DailyBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string AppUserId {  get; set; }
        public AppUser appUser { get; set; }
        public int DurationDays { get; set; }
        public Gender Gender { get; set; }
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<CampaignCountry> CampaignCountries    { get; set; } = new List<CampaignCountry>();
    }
}
