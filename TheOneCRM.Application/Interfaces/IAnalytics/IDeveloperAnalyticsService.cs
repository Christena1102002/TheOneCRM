using System.Collections.Generic;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Analytics;

namespace TheOneCRM.Application.Interfaces.IAnalytics
{
    public interface IDeveloperAnalyticsService
    {
        // الكروت + المميّزون
        Task<DeveloperAnalyticsSummaryDto> GetSummaryAsync();

        // إحصائيات المطورين الفردية (الجدول + شارت الأداء)
        Task<List<DeveloperStatItemDto>> GetDeveloperStatsAsync();

        // الشارتس (إنجاز المهام عبر الزمن + تقدّم المشاريع)
        Task<AnalyticsChartsDto> GetChartsAsync();

        // تحليلات الأخطاء (توزيع حسب المشروع + مفتوحة vs محلولة شهريًا)
        Task<BugAnalyticsDto> GetBugAnalyticsAsync();
    }
}
