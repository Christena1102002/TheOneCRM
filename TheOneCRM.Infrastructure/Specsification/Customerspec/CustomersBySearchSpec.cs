using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class CustomersBySearchSpec : BaseSpecification<Customer>
    {
        public CustomersBySearchSpec(string searchTerm) : base(c =>
                string.IsNullOrEmpty(searchTerm) ||
                c.FullName.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm)
            )
        {
            AddInclude(c => c.campaigns);
            AddInclude(c => c.AssignedTo);
            AddInclude("customerServices.Service");

            // الترتيب: الأحدث الأول
            ApplyOrderByDescending(c => c.CreatedAt);
        }
    }
}
