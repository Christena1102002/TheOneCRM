using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Helper;
using TheOneCRM.Application.Interfaces.ICompanySettings;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.CompanySettings;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CompanySettingsController : ControllerBase
    {
        private readonly ICompanySettingsService _settingsService;

        public CompanySettingsController(ICompanySettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: api/CompanySettings — بترجّع الإعدادات (الصف الوحيد) أو null
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _settingsService.GetAsync();
            return Ok(new ApiResponse(200, "Company settings retrieved successfully", result));
        }

        // PUT: api/CompanySettings/Update — يضيف لو مش موجود، يعدّل لو موجود (multipart للشعار والأيقونة)
        [HttpPut("Update")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Update(
            [FromForm] UpdateCompanySettingsDto dto,
            IFormFile? logo,
            IFormFile? favicon)
        {
            string? logoUrl = logo is { Length: > 0 }
                ? await DocumentSettings.UploadFileAsync(logo, "CompanySettings")
                : null;

            string? faviconUrl = favicon is { Length: > 0 }
                ? await DocumentSettings.UploadFileAsync(favicon, "CompanySettings")
                : null;

            var result = await _settingsService.UpsertAsync(dto, logoUrl, faviconUrl);
            return Ok(new ApiResponse(200, "Company settings saved successfully", result));
        }
    }
}
