using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    // عملاء تمت استشارتهم بواسطة موظف دعم معين (رجعوا منه للمبيعات)
    public class ConsultedBySupportSpec : BaseSpecification<Customer>
    {
        public ConsultedBySupportSpec(string supportUserId)
            : base(c => c.AssignmentHistory.Any(
                h => h.FromUserId == supportUserId && h.FromRole == UserRoles.Support))
        {
            AddInclude(c => c.AssignmentHistory);
        }
    }
}
