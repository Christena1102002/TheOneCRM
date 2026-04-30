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
        public string? Source { get; set; }
        public string? CampaignName { get; set; }
        public List<string> Services { get; set; } = new();
        public string Status { get; set; }
        public string? SalesPersonName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
