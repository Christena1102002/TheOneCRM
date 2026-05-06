using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.CampaignDto
{
    public class CampaignDetailsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int ChannelSourceId { get; set; }
        public string ChannelSource { get; set; }
        public int DurationDays { get; set; }

        public decimal Budget { get; set; }
        public decimal DailyBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal Spent { get; set; }             // تم الإنفاق (محسوبة)
        public decimal Remaining { get; set; }         // المتبقي (محسوبة)
        public decimal SpentPercentage { get; set; }   // النسبة المئوية للإنفاق
        public int DaysElapsed { get; set; }           // الأيام اللي عدّت
        public int DaysRemaining { get; set; }         // الأيام المتبقية
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string AppUserId { get; set; }
        public string appUserName { get; set; }
        public DateTime createdAt { get; set; }

        public string Gender { get; set; }
        public ICollection<CampaignCountryDto> Countries { get; set; }
    }
}
