namespace TheOneCRM.Domain.Models.DTOs.CompanySettings
{
    // الحقول النصية (الشعار والأيقونة بتتبعتوا كـ IFormFile منفصلين في الـ controller)
    public class UpdateCompanySettingsDto
    {
        public string CompanyName { get; set; } = null!;
        public string? TradeName { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Mobile { get; set; }
        public string? Website { get; set; }
        public string Address { get; set; } = null!;
        public string? CommercialRegistration { get; set; }
        public string? TaxNumber { get; set; }
        public string DefaultCurrency { get; set; } = "SAR";
    }
}
