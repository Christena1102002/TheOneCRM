using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class CustomerActivity : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public CustomerActivityType ActivityType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }

        // للمكالمات فقط (ContactAttempted)
        public ContactResult? ContactResult { get; set; }

        // لتغيير الحالة فقط (StatusChanged)
        public CustomerStatus? FromStatus { get; set; }
        public CustomerStatus? ToStatus { get; set; }

        // ربط بكيان تاني لو محتاج (مثلاً QuotationId)
        public int? RelatedEntityId { get; set; }
    }
}
