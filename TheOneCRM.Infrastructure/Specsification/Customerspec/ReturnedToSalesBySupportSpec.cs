using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // العملاء اللي رجعهم موظف الدعم للمبيعات
    public class ReturnedToSalesBySupportSpec : BaseSpecification<CustomerActivity>
    {
        public ReturnedToSalesBySupportSpec(string supportUserId)
            : base(a =>
                a.ActivityType == CustomerActivityType.ReturnedToSales
                && a.CreatedById == supportUserId)
        {
        }
    }
}
