using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos
{
    public class UpdatePriceQuotationDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "قيمة الخصم لا يمكن أن تكون سالبة")]
        public decimal Discount { get; set; }

        public string? Notes { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "يجب إضافة بند واحد على الأقل")]
        public List<UpdatePriceQuotationDetailsDto> Items { get; set; }
            = new List<UpdatePriceQuotationDetailsDto>();
    }
}
