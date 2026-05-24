using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.Dashboard;
using TheOneCRM.Domain.Models.DTOs.Tasks;

namespace TheOneCRM.Application.Interfaces.ITasks
{
    public interface ITaskService
    {
        // ===== المدير =====
        Task<int> CreateTaskAsync(CreateTaskDto dto, string managerId);
        Task<TaskResponseDto> GetTaskByIdAsync(int id);
        Task<Pagination<TaskResponseDto>> GetTasksAsync(TaskParams p);
        Task UpdateTaskAsync(int id, UpdateTaskDto dto);
        Task DeleteTaskAsync(int id);

        // ===== الـ developer =====
        Task<Pagination<TaskResponseDto>> GetMyTasksAsync(TaskParams p, string developerId);
        Task<TaskResponseDto> GetMyTaskByIdAsync(int id, string developerId);
        Task UpdateMyTaskStatusAsync(int id, UpdateTaskStatusDto dto, string developerId);

        // ===== الإحصائيات (Dashboard) =====
        Task<DashboardStatsDto> GetDeveloperStatisticsAsync(string developerId);
        Task<DashboardStatsDto> GetAdminStatisticsAsync();

        // ===== لوحة التحكم (Control Panel) =====
        Task<ControlPanelDto> GetDeveloperControlPanelAsync(string developerId);
        Task<ControlPanelDto> GetAdminControlPanelAsync();
    }
}
