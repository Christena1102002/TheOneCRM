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
        Task<StatisticsMarketingDto> GetStatisticsAsync();
        Task<MarketingDashboardDto> GetDashboardStats();
        Task<List<DailyLeadsDto>> GetPotentialCustomersLast7DaysAsync();

        Task<List<SourcePerformanceDto>> GetSourcePerformanceAsync();
    }
}
