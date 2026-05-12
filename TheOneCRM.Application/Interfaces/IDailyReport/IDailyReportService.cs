using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.DailyReports;

namespace TheOneCRM.Application.Interfaces.IDailyReport
{
    public interface IDailyReportService
    {
        Task<DailyReportResponseDto> CreateDailyReportAsync(CreateDailyReportDto dto, string userId);
        Task<DailyReportResponseDto> UpdateDailyReportAsync(int id, UpdateDailyReportDto dto, string userId);
        Task DeleteDailyReportAsync(int id, string userId, bool isAdmin);
        Task<DailyReportResponseDto> GetDailyReportByIdAsync(int id);
        Task<Pagination<DailyReportListItemDto>> GetDailyReportsAsync(DailyReportQueryParams p);
        Task<DailyReportResponseDto?> GetMyReportByDateAsync(string userId, DateTime date);
    }
}
