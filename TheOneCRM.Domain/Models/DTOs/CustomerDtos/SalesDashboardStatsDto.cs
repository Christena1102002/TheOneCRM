using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class SalesDashboardStatsDto
    {
        public int BuyerCustomers { get; set; }         // عملائي النشطين
        public int CallsToday { get; set; }              // مكالمات النهاردة
        public int UpcomingFollowUps { get; set; }       // متابعات قادمة
        public int OverdueFollowUps { get; set; }        // متابعات فاتت موعدها
        public int TransferredToSupport { get; set; }    // عملاء حولّتهم لـ Support
        public int ReceivedFromMarketing { get; set; }   // عملاء استلمتهم من Marketing
    }
}
