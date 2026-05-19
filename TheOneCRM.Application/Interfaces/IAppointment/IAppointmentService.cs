using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Appointments;
using TheOneCRM.Infrastructure.Specsification.Appointments;

namespace TheOneCRM.Application.Interfaces.IAppointment
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto> CreateAppointmentAsync(CreateAppointmentDto dto, string createdByUserId);

        Task<AppointmentResponseDto> UpdateAppointmentAsync(int id,UpdateAppointmentDto dto,string currentUserId, string currentUserRole);
        Task<AppointmentResponseDto> GetAppointmentByIdAsync(int id);
        Task<bool> DeleteAppointmentAsync(int id, string currentUserId, string currentUserRole);

        // ⭐ الجديدين
        Task<(IReadOnlyList<AppointmentResponseDto> Items, int TotalCount)> GetAppointmentsAsync(AppointmentSpecParams specParams, string currentUserId);
        Task<AppointmentStatsDto> GetStatsAsync(string currentUserId);
    }
}

