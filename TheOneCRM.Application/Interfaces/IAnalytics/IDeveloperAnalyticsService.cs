using System.Collections.Generic;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Analytics;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.Application.Interfaces.IAnalytics
{
    public interface IDeveloperAnalyticsService
    {
        // الفلترة: مطوّر + مشروع + فترة زمنية (DeveloperAnalyticsParams)

        // الكروت + المميّزون
        Task<DeveloperAnalyticsSummaryDto> GetSummaryAsync(DeveloperAnalyticsParams p);

        // إحصائيات المطورين الفردية (الجدول + شارت الأداء)
        Task<List<DeveloperStatItemDto>> GetDeveloperStatsAsync(DeveloperAnalyticsParams p);

        // الشارتس (إنجاز المهام عبر الزمن + تقدّم المشاريع)
        Task<AnalyticsChartsDto> GetChartsAsync(DeveloperAnalyticsParams p);

        // تحليلات الأخطاء (توزيع حسب المشروع + مفتوحة vs محلولة شهريًا)
        Task<BugAnalyticsDto> GetBugAnalyticsAsync(DeveloperAnalyticsParams p);

        // قائمة المشاريع للـ dropdown (مشاريع المطوّر المختار أو الكل)
        Task<List<StatusClientDto>> GetProjectOptionsAsync(string? developerId);
    }
}
