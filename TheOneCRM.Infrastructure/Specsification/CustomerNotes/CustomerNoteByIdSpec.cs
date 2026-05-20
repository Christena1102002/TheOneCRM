using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.CustomerNotes
{
    public class CustomerNoteByIdSpec : BaseSpecification<CustomerNote>
    {
        public CustomerNoteByIdSpec(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Customer);
            AddInclude(x => x.MarketingCreatedBy);
            AddInclude(x => x.SalesCreatedBy);
            AddInclude(x => x.SupportCreatedBy);
        }
    }
}
