using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // تعد المكالمات الناجحة (Answered) اللي حصلت النهارده
    // بغض النظر عن الحالة الحالية للعميل
    public class CallsTodayBySalesPersonSpec : BaseSpecification<CustomerActivity>
    {
        public CallsTodayBySalesPersonSpec(string? salesPersonId, DateTime today, DateTime tomorrow)
            : base(a =>
                a.ActivityType == CustomerActivityType.ContactAttempted
                && a.ContactResult == ContactResult.Answered
                && a.CreatedAt >= today
                && a.CreatedAt < tomorrow
                && (salesPersonId == null || a.CreatedById == salesPersonId)
            )
        {
        }
    }
}
