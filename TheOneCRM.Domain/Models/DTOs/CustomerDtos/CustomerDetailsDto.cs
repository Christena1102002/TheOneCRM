using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerDetailsDto
    {
        public int Id { get; set; }

        // البيانات الأساسية
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? Notes { get; set; }

        // المصدر
        public int Source { get; set; }                  // الـ ID للـ enum (للفورم)
        public string SourceName { get; set; }           // الاسم للعرض

        // الحملة
        public int? CampaignId { get; set; }
        public string? CampaignName { get; set; }

        // الخدمات (IDs للفورم + Names للعرض)
        public List<int> ServiceIds { get; set; } = new();
        public List<ServiceItemDto> Services { get; set; } = new();

        // الحالة
        public int Status { get; set; }
        public string StatusName { get; set; }

        // المندوب
        public string? SalesPersonId { get; set; }
        public string? SalesPersonName { get; set; }
        public DateTime? AssignedAt { get; set; }

        // التواريخ
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ServiceItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
