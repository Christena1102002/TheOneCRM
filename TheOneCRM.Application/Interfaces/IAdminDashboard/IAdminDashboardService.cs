using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.AdminDashboard;

namespace TheOneCRM.Application.Interfaces.IAdminDashboard
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();
    }
}
