using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Analytics
{
    public class DeveloperAnalyticsParams
    {
        // فلترة بمطوّر (للأدمن) — المطوّر نفسه بيتفرض عليه id بتاعه في الكنترولر
        public string? DeveloperId { get; set; }

        // فلترة بمشروع
        public int? ProjectId { get; set; }

        // الفترة الزمنية (افتراضي: الشهر الحالي)
        public AnalyticsPeriod Period { get; set; } = AnalyticsPeriod.CurrentMonth;
    }
}
