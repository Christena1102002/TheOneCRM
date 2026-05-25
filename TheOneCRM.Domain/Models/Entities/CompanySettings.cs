using System;

namespace TheOneCRM.Domain.Models.Entities
{
    // إعدادات الشركة/النظام — صف واحد بس
    public class CompanySettings : BaseEntity
    {
        // معلومات الشركة
        public string CompanyName { get; set; } = null!;     // اسم الشركة *
        public string? TradeName { get; set; }               // الاسم التجاري
        public string Email { get; set; } = null!;           // البريد الإلكتروني *
        public string Phone { get; set; } = null!;           // رقم الهاتف *
        public string? Mobile { get; set; }                  // رقم الجوال
        public string? Website { get; set; }                 // الموقع الإلكتروني
        public string Address { get; set; } = null!;         // العنوان *
        public string? CommercialRegistration { get; set; }  // السجل التجاري
        public string? TaxNumber { get; set; }               // الرقم الضريبي

        // الهوية
        public string? LogoUrl { get; set; }                 // شعار النظام
        public string? FaviconUrl { get; set; }              // أيقونة النظام (Favicon)

        // العملة الافتراضية (مثلاً SAR)
        public string DefaultCurrency { get; set; } = "SAR";
    }
}
