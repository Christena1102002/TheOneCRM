using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.DailyReports
{
    public class CreateDailyReportDto
    {
        [Required(ErrorMessage = "التاريخ مطلوب")]
        public DateTime ReportDate { get; set; }

        [Required(ErrorMessage = "المهام المنجزة مطلوبة")]
        [MaxLength(4000)]
        public string CompletedTasks { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? TasksInProgress { get; set; }

        [MaxLength(4000)]
        public string? PlannedTasks { get; set; }

        [MaxLength(4000)]
        public string? Challenges { get; set; }

        [Range(0, 24, ErrorMessage = "ساعات العمل لازم تكون بين 0 و 24")]
        public decimal WorkHours { get; set; }

        [MaxLength(2000)]
        public string? AdditionalNotes { get; set; }
    }
}
