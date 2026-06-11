using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class AssignedCustomersWithNotesSpec : BaseSpecification<Customer>
    {
        public AssignedCustomersWithNotesSpec(string userId)
            : base(c => c.AssignedToId == userId)
        {
            AddInclude(c => c.Notes);
        }
    }
}
