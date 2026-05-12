using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.DailyReports
{
    public class DailyReportQueryParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; } // "dateAsc", "dateDesc", "hoursAsc", "hoursDesc"
    }
}
