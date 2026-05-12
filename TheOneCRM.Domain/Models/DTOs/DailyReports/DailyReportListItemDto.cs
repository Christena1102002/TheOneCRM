using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.DailyReports
{
    public class DailyReportListItemDto
    {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal WorkHours { get; set; }
        public string CompletedTasksPreview { get; set; } = string.Empty; // أول 100 حرف
        public DateTime CreatedAt { get; set; }
    }
}
