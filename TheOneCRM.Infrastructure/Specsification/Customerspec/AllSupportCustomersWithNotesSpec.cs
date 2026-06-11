using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // كل العملاء المعينين حالياً لأي موظف دعم (بانتظار الاستشارة)
    public class AllSupportCustomersWithNotesSpec : BaseSpecification<Customer>
    {
        public AllSupportCustomersWithNotesSpec()
            : base(c => c.IsSalesToSupport && !c.IsSupportToSales)
        {
            AddInclude(c => c.Notes);
        }
    }
}
