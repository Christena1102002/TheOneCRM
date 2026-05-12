using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.DailyReports
{
    public class DailyReportResponseDto
    {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string CompletedTasks { get; set; } = string.Empty;
        public string? TasksInProgress { get; set; }
        public string? PlannedTasks { get; set; }
        public string? Challenges { get; set; }
        public decimal WorkHours { get; set; }
        public string? AdditionalNotes { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
