using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CompanySettings;

namespace TheOneCRM.Application.Interfaces.ICompanySettings
{
    public interface ICompanySettingsService
    {
        // بترجّع الصف الوحيد (أو null لو لسه متعملش)
        Task<CompanySettingsResponseDto?> GetAsync();

        // بتضيف الإعدادات لو مش موجودة، وبتعمل update لو موجودة
        Task<CompanySettingsResponseDto> UpsertAsync(
            UpdateCompanySettingsDto dto, string? logoUrl, string? faviconUrl);
    }
}
