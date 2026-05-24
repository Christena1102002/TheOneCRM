using System.Collections.Generic;

namespace TheOneCRM.Domain.Models.DTOs.Resources
{
    public class WorkloadOverviewDto
    {
        // ===== الكروت =====
        public int AvailableDevelopers { get; set; }   // مطورون متاحون
        public int OverloadedDevelopers { get; set; }  // مطورون محمّلون
        public int AverageWorkload { get; set; }       // متوسط حمل العمل (%)
        public int TotalDevelopers { get; set; }       // إجمالي المطورين

        // نظرة عامة على الفريق
        public List<DeveloperWorkloadDto> Developers { get; set; } = new();
    }

    public class DeveloperWorkloadDto
    {
        public string DeveloperId { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Specialty { get; set; }   // تخصّص المطوّر (من AppUser.Specialty)

        public int TasksCount { get; set; }      // عدد المهام
        public int UsedHours { get; set; }       // الساعات المستخدمة
        public int AvailableHours { get; set; }  // الساعات المتاحة
        public int CapacityHours { get; set; }   // السعة الأسبوعية (40)
        public int WorkloadPercent { get; set; } // نسبة حمل العمل
    }

    public class WorkloadDistributionItemDto
    {
        public string DeveloperId { get; set; } = null!;
        public string? FullName { get; set; }
        public int TasksCount { get; set; }
        public int WorkloadPercent { get; set; }
    }
}
