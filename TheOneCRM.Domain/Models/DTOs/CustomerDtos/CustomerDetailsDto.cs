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
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? CampanyName{ get; set; }
        public string? Notes { get; set; }

        // الحملة
        public int? CampaignId { get; set; }


        // الخدمات (IDs للفورم + Names للعرض)
        public string? Source { get; set; }
        public string? CampaignName { get; set; }
        public List<string> Services { get; set; } = new();

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
