using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Application.Interfaces.IDailyReport;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.DailyReports;
using TheOneCRM.Infrastructure.Specsification.DailyReports;

namespace TheOneCRM.Application.Services.Report
{
    public class DailyReportService : IDailyReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DailyReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DailyReportResponseDto> CreateDailyReportAsync(CreateDailyReportDto dto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("المستخدم غير مصرح له");

            // تحقق إن الموظف معملش تقرير لنفس اليوم
            var existing = await _unitOfWork.Repository<DailyReport>()
                .GetEntityWithSpec(new DailyReportsWithUserSpec(userId, dto.ReportDate));

            if (existing != null)
                throw new InvalidOperationException($"يوجد تقرير بالفعل بتاريخ {dto.ReportDate:yyyy-MM-dd}");

            var report = new DailyReport
            {
                ReportDate = dto.ReportDate.Date,
                CompletedTasks = dto.CompletedTasks.Trim(),
                TasksInProgress = dto.TasksInProgress?.Trim(),
                PlannedTasks = dto.PlannedTasks?.Trim(),
                Challenges = dto.Challenges?.Trim(),
                WorkHours = dto.WorkHours,
                AdditionalNotes = dto.AdditionalNotes?.Trim(),
                UserId = userId
            };

            await _unitOfWork.Repository<DailyReport>().AddAsync(report);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new InvalidOperationException("فشل حفظ التقرير اليومي");

            return await GetDailyReportByIdAsync(report.Id);
        }

        public async Task<DailyReportResponseDto> UpdateDailyReportAsync(int id, UpdateDailyReportDto dto, string userId)
        {
            var report = await _unitOfWork.Repository<DailyReport>().GetByIdAsync(id);
            if (report == null)
                throw new KeyNotFoundException($"التقرير اليومي رقم {id} غير موجود");

            // الموظف يقدر يعدّل تقريره فقط
            if (report.UserId != userId)
                throw new InvalidOperationException("غير مصرح لك بتعديل هذا التقرير");

            // لو غيّر التاريخ، تحقق من عدم التعارض
            if (report.ReportDate.Date != dto.ReportDate.Date)
            {
                var clash = await _unitOfWork.Repository<DailyReport>()
                    .GetEntityWithSpec(new DailyReportsWithUserSpec(userId, dto.ReportDate));

                if (clash != null && clash.Id != id)
                    throw new InvalidOperationException($"يوجد تقرير آخر بتاريخ {dto.ReportDate:yyyy-MM-dd}");
            }

            report.ReportDate = dto.ReportDate.Date;
            report.CompletedTasks = dto.CompletedTasks.Trim();
            report.TasksInProgress = dto.TasksInProgress?.Trim();
            report.PlannedTasks = dto.PlannedTasks?.Trim();
            report.Challenges = dto.Challenges?.Trim();
            report.WorkHours = dto.WorkHours;
            report.AdditionalNotes = dto.AdditionalNotes?.Trim();

            _unitOfWork.Repository<DailyReport>().Update(report);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new InvalidOperationException("فشل تحديث التقرير اليومي");

            return await GetDailyReportByIdAsync(id);
        }

        public async Task DeleteDailyReportAsync(int id, string userId, bool isAdmin)
        {
            var report = await _unitOfWork.Repository<DailyReport>().GetByIdAsync(id);
            if (report == null)
                throw new KeyNotFoundException($"التقرير اليومي رقم {id} غير موجود");

            // غير الـ Admin يقدر يحذف تقريره فقط
            if (!isAdmin && report.UserId != userId)
                throw new InvalidOperationException("غير مصرح لك بحذف هذا التقرير");

            _unitOfWork.Repository<DailyReport>().Delete(report);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new InvalidOperationException("فشل حذف التقرير اليومي");
        }

        public async Task<DailyReportResponseDto> GetDailyReportByIdAsync(int id)
        {
            var spec = new DailyReportsWithUserSpec(id);
            var report = await _unitOfWork.Repository<DailyReport>().GetEntityWithSpec(spec);

            if (report == null)
                throw new KeyNotFoundException($"التقرير اليومي رقم {id} غير موجود");

            return MapToResponseDto(report);
        }

        public async Task<Pagination<DailyReportListItemDto>> GetDailyReportsAsync(DailyReportQueryParams p)
        {
            var spec = new DailyReportsWithUserSpec(p);
            var countSpec = new DailyReportsWithUserSpec(p, count: true);

            var totalItems = await _unitOfWork.Repository<DailyReport>().CountAsync(countSpec);
            var data = await _unitOfWork.Repository<DailyReport>().ListAsync(spec);

            var items = data.Select(r => new DailyReportListItemDto
            {
                Id = r.Id,
                ReportDate = r.ReportDate,
                UserName = r.appUser?.UserName ?? string.Empty,
                WorkHours = r.WorkHours,
                CompletedTasksPreview = r.CompletedTasks.Length > 100
                    ? r.CompletedTasks.Substring(0, 100) + "..."
                    : r.CompletedTasks,
                CreatedAt = r.CreatedAt
            }).ToList();

            return new Pagination<DailyReportListItemDto>(p.PageIndex, p.PageSize, totalItems, items);
        }

        public async Task<DailyReportResponseDto?> GetMyReportByDateAsync(string userId, DateTime date)
        {
            var spec = new DailyReportsWithUserSpec(userId, date);
            var report = await _unitOfWork.Repository<DailyReport>().GetEntityWithSpec(spec);
            return report == null ? null : MapToResponseDto(report);
        }

        private static DailyReportResponseDto MapToResponseDto(DailyReport r) => new()
        {
            Id = r.Id,
            ReportDate = r.ReportDate,
            CompletedTasks = r.CompletedTasks,
            TasksInProgress = r.TasksInProgress,
            PlannedTasks = r.PlannedTasks,
            Challenges = r.Challenges,
            WorkHours = r.WorkHours,
            AdditionalNotes = r.AdditionalNotes,
            UserId = r.UserId,
            UserName = r.appUser?.UserName ?? string.Empty,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        };
    }
}