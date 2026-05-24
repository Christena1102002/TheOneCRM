using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.Projects;

namespace TheOneCRM.Application.Interfaces.IProjects
{
    public interface IProjectService
    {
        Task<ProjectCustomerLookupDto> GetCustomerLookupAsync(int customerId);
        Task<int> CreateProjectAsync(CreateProjectDto dto, string userId);
        Task<ProjectResponseDto> GetProjectByIdAsync(int id);
        Task<Pagination<ProjectResponseDto>> GetProjectsAsync(ProjectParams p);
        Task UpdateProjectAsync(int id, UpdateProjectDto dto);
        Task DeleteProjectAsync(int id);

        // الـ developer: مشاريعه هو بس
        Task<Pagination<ProjectResponseDto>> GetMyProjectsAsync(ProjectParams p, string developerId);
        Task<ProjectResponseDto> GetMyProjectByIdAsync(int id, string developerId);

        // المشاريع النشطة
        Task<Pagination<ProjectResponseDto>> GetActiveProjectsAsync(ProjectParams p);
        Task<Pagination<ProjectResponseDto>> GetMyActiveProjectsAsync(ProjectParams p, string developerId);
    }
}
