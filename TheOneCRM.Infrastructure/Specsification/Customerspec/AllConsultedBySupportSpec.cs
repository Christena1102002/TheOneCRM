using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // كل العملاء اللي تمت استشارتهم (رجعوا للمبيعات بعد الدعم)
    public class AllConsultedBySupportSpec : BaseSpecification<Customer>
    {
        public AllConsultedBySupportSpec()
            : base(c => c.IsConsulted)
        {
        }
    }
}
