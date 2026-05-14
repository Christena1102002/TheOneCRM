using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class PriceQuotationDetails :BaseEntity
    {
        // عرض السعر الذي ينتمي إليه هذا البند
        public int PriceQuotationId { get; set; }
        public PriceQuotation PriceQuotation { get; set; }

        // الخدمة المختارة
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        // السعر المتفق عليه مع العميل لهذا العرض
        public decimal UnitPrice { get; set; }

        // الكمية
        public int Quantity { get; set; }

        // الإجمالي = UnitPrice * Quantity
        public decimal TotalPrice { get; set; }
    }
}
