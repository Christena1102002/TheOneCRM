using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? Notes { get; set; }
        public int? CampaignId { get; set; }
        public List<int>? ServiceIds { get; set; }
    }
}
