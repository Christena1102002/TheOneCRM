using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos
{
    public class PriceQuotationListDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CampanyName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal NetTotal { get; set; }
        public int ItemsCount { get; set; }
        public string? Notes { get; set; }
    }
}
