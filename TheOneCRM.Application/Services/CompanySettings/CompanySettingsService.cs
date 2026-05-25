using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Application.Interfaces.ICompanySettings;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CompanySettings;
using Entity = TheOneCRM.Domain.Models.Entities.CompanySettings;

namespace TheOneCRM.Application.Services.CompanySettings
{
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanySettingsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanySettingsResponseDto?> GetAsync()
        {
            var settings = await GetSingleAsync();
            return settings is null ? null : _mapper.Map<CompanySettingsResponseDto>(settings);
        }

        public async Task<CompanySettingsResponseDto> UpsertAsync(
            UpdateCompanySettingsDto dto, string? logoUrl, string? faviconUrl)
        {
            var settings = await GetSingleAsync();

            if (settings is null)
            {
                // مفيش إعدادات → أنشئ صف جديد
                settings = _mapper.Map<Entity>(dto);
                settings.LogoUrl = logoUrl;
                settings.FaviconUrl = faviconUrl;

                await _unitOfWork.Repository<Entity>().AddAsync(settings);
            }
            else
            {
                // موجود → حدّث، وبس غيّر الشعار/الأيقونة لو اترفع جديد
                _mapper.Map(dto, settings);
                if (!string.IsNullOrEmpty(logoUrl)) settings.LogoUrl = logoUrl;
                if (!string.IsNullOrEmpty(faviconUrl)) settings.FaviconUrl = faviconUrl;
                settings.UpdatedAt = System.DateTime.UtcNow;

                _unitOfWork.Repository<Entity>().Update(settings);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CompanySettingsResponseDto>(settings);
        }

        // الصف الوحيد (أقدم/أول صف)
        private async Task<Entity?> GetSingleAsync()
        {
            var all = await _unitOfWork.Repository<Entity>().ListAllAsync();
            return all.OrderBy(s => s.Id).FirstOrDefault();
        }
    }
}
