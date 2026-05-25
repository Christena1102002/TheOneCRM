using System;

namespace TheOneCRM.Domain.Models.DTOs.CompanySettings
{
    public class CompanySettingsResponseDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? TradeName { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Mobile { get; set; }
        public string? Website { get; set; }
        public string Address { get; set; } = null!;
        public string? CommercialRegistration { get; set; }
        public string? TaxNumber { get; set; }
        public string? LogoUrl { get; set; }
        public string? FaviconUrl { get; set; }
        public string DefaultCurrency { get; set; } = "SAR";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
