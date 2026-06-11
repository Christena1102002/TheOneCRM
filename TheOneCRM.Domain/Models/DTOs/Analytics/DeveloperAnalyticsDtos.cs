using System.Collections.Generic;

namespace TheOneCRM.Domain.Models.DTOs.Analytics
{
    // ===== الكروت العلوية + المميّزون =====
    public class DeveloperAnalyticsSummaryDto
    {
        public int ProductivityRate { get; set; }            // معدل الإنتاجية %
        public int ProductivityChangePercent { get; set; }   // التغيّر عن الأسبوع الماضي

        public int ResolvedBugs { get; set; }                // الأخطاء المحلولة
        public int OpenBugs { get; set; }                    // الأخطاء المفتوحة

        public double AvgCompletionTimeHours { get; set; }   // متوسط وقت الإنجاز (ساعات/مهمة)

        public int CompletedTasks { get; set; }              // المهام المكتملة
        public int CompletedTasksChangePercent { get; set; } // التغيّر عن الشهر الماضي

        public TopDeveloperDto? MostProductive { get; set; } // الأكثر إنتاجية
        public TopDeveloperDto? Fastest { get; set; }        // أسرع إنجاز
        public TopDeveloperDto? TopPerformer { get; set; }   // أعلى أداء
    }

    public class TopDeveloperDto
    {
        public string DeveloperId { get; set; } = null!;
        public string? FullName { get; set; }
        public double Value { get; set; }     // القيمة (نسبة / ساعات / عدد)
        public string Label { get; set; } = null!; // وصف القيمة بالعربي
    }

    // ===== إحصائيات المطورين الفردية (الجدول + شارت الأداء) =====
    public class DeveloperStatItemDto
    {
        public string DeveloperId { get; set; } = null!;
        public string? FullName { get; set; }
        public int CompletedTasks { get; set; }              // المهام المكتملة
        public double AvgCompletionTimeHours { get; set; }   // متوسط وقت الإنجاز
        public int ResolvedBugs { get; set; }                // الأخطاء المحلولة
        public int CurrentWorkloadPercent { get; set; }      // حمل العمل الحالي
        public int ProductivityPercent { get; set; }         // الإنتاجية
    }

    // ===== الشارتس =====
    public class AnalyticsChartsDto
    {
        public List<TaskCompletionPointDto> TaskCompletionOverTime { get; set; } = new();
        public List<ProjectProgressItemDto> ProjectsProgress { get; set; } = new();
    }

    public class TaskCompletionPointDto
    {
        public string Day { get; set; } = null!; // Sun .. Sat
        public int Completed { get; set; }       // مكتملة
        public int Pending { get; set; }         // معلقة
    }

    public class ProjectProgressItemDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public int Progress { get; set; }        // نسبة الإنجاز %
    }

    // ===== تحليلات الأخطاء =====
    public class BugAnalyticsDto
    {
        public List<BugByProjectDto> DistributionByProject { get; set; } = new();
        public List<MonthlyBugDto> MonthlyOpenVsResolved { get; set; } = new();
    }

    public class BugByProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public int Count { get; set; }
        public int Percent { get; set; }
    }

    public class MonthlyBugDto
    {
        public string Month { get; set; } = null!;
        public int Open { get; set; }
        public int Resolved { get; set; }
    }

    // ===== كل التحليلات في رد واحد =====
    public class FullDeveloperAnalyticsDto
    {
        public DeveloperAnalyticsSummaryDto Summary { get; set; } = null!;
        public List<DeveloperStatItemDto> DeveloperStats { get; set; } = new();
        public AnalyticsChartsDto Charts { get; set; } = null!;
        public BugAnalyticsDto BugAnalytics { get; set; } = null!;
    }
}
