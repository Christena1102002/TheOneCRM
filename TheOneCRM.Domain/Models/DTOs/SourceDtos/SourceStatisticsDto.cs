using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.SourceDtos
{
    public class SourceStatisticsDto
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }

        public int BuyersCount { get; set; }
        public int NotBuyersCount { get; set; }
        public int TotalCustomers { get; set; }
    }
}
