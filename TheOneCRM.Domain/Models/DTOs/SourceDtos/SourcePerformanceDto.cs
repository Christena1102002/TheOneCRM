using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.SourceDtos
{
    public class SourcePerformanceDto
    {
        public int ChannelSourceId { get; set; }
        public string SourceName { get; set; } 
        public int CustomersCount { get; set; }
        public decimal TotalBudget { get; set; }
    }
}
