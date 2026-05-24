using System.Collections.Generic;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Resources;

namespace TheOneCRM.Application.Interfaces.IResources
{
    public interface IResourceManagementService
    {
        // نظرة عامة على أحمال الفريق + الكروت (للأدمن)
        Task<WorkloadOverviewDto> GetTeamWorkloadOverviewAsync(WorkloadParams p);

        // توزيع أحمال العمل (للأدمن)
        Task<List<WorkloadDistributionItemDto>> GetWorkloadDistributionAsync();
    }
}
