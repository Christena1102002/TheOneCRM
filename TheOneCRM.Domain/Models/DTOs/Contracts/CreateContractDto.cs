using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Contracts
{
    public class CreateContractDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CustomerId { get; set; }
     
        public ContractStatus Status { get; set; } = ContractStatus.Active;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
