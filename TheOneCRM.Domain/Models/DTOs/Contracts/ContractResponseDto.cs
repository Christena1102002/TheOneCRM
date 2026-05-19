using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Contracts
{
    public class ContractResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        // بيانات العميل
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }

        // اللي أنشأ العقد
        public string? CreatedById { get; set; }
        public string? CreatedByName { get; set; }

        // الحالة
        public ContractStatus Status { get; set; }
        public string StatusName { get; set; } = null!;

        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
