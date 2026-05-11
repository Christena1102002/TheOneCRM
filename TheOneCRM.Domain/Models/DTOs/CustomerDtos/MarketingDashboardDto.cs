using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class MarketingDashboardDto
    {
        public decimal CustomerCost { get; set; }         // تكلفة العميل
        public int ActiveCampaigns { get; set; }          // الحملات النشطة
        public int TodayCustomers { get; set; }           // عملاء اليوم
        public int TotalPotentialCustomers { get; set; }  // إجمالي العملاء المحتملين
    }
}
