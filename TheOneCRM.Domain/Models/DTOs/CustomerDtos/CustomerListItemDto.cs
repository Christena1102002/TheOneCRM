using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerListItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Source { get; set; }
        public string? CampaignName { get; set; }
        public List<ServiceItemDto> Services { get; set; } = new();
        public string Status { get; set; }
        public string? SalesPersonId { get; set; }
        public string? SalesPersonName { get; set; }
        public string? SupportPersonName { get; set; }
        public string? SupportPersonId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastFollowUpDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public bool IsMarketingToSales { get; set; }
        public bool IsSalesToSupport { get; set; }
        public bool IsSupportToSales { get; set; }
        public bool IsConsulted { get; set; }

        public string? NoteMarketing { get; set; }
        public string? NoteSales { get; set; }
        public string? NoteSupport { get; set; }


        public string? NotBuyerReason { get; set; }

        // تاريخ العميل الكامل
        public List<CustomerActivityDto> Activities { get; set; } = new();
    }
}
