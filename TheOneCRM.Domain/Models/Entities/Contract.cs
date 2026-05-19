using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Contract : BaseEntity
    {
        // اسم/عنوان العقد
        public string Title { get; set; } = null!;

        // وصف العقد
        public string? Description { get; set; }

        // ربط بالعميل (one-to-many)
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string? CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        // حالة العقد
        public ContractStatus Status { get; set; }
        // القيمة المالية
        public decimal Price { get; set; }
        // التواريخ
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }



    }
}
