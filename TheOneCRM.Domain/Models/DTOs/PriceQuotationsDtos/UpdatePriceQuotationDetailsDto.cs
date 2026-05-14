using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos
{
    public class UpdatePriceQuotationDetailsDto
    {
        [Required]
        public int ServiceId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "السعر لا يمكن أن يكون سالباً")]
        public decimal UnitPrice { get; set; }

       
        public int Quantity { get; set; }
    }
}
