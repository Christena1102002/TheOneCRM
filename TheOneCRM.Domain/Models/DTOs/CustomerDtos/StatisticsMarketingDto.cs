using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class StatisticsMarketingDto
    {
        public int TotalCustomers { get; set; }
        public int BuyerCustomers { get; set; }
        public int NotBuyerCustomers { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
