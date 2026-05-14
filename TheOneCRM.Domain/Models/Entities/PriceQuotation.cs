using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class PriceQuotation :BaseEntity
    {
        // العميل الذي يخصه عرض السعر
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        // مجموع (السعر × الكمية) لكل البنود قبل الخصم
        public decimal SubTotal { get; set; }

        // قيمة الخصم
        public decimal Discount { get; set; }

        // الصافي = SubTotal - Discount
        public decimal NetTotal { get; set; }

        // ملاحظات
        public string? Notes { get; set; }

        // تفاصيل الخدمات داخل عرض السعر
        public ICollection<PriceQuotationDetails> Items { get; set; }
            = new List<PriceQuotationDetails>();
    }
}
