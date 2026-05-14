using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos
{
    public class PriceQuotationResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CampanyName { set; get; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal NetTotal { get; set; }
        public string? Notes { get; set; }
        public List<PriceQuotationDetailsResponseDto> Items { get; set; }
            = new List<PriceQuotationDetailsResponseDto>();
    }
}
