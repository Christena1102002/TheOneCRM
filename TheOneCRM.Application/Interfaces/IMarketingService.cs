using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;

namespace TheOneCRM.Application.Interfaces
{
    public interface IMarketingService
    {
        Task<StatisticsMarketingDto> GetStatisticsAsync(string? ownerId);
        Task<MarketingDashboardDto> GetDashboardStats(string? ownerId);
        Task<List<DailyLeadsDto>> GetPotentialCustomersLast7DaysAsync(string? ownerId);

        Task<List<SourcePerformanceDto>> GetSourcePerformanceAsync(string? ownerId);
    }
}
