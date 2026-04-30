using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CreateCustomerDto
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string CompanyName { get; set; }
        public string Notes { get; set; }
        public int? CampaignId { get; set; }

        public List<int>? ServiceIds { get; set; } = new();

      
        public bool AssignToSalesTeam { get; set; } = false;
        public string? SalesPersonId { get; set; }
    }
}
