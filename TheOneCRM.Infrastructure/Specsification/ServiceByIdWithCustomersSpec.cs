using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class ServiceByIdWithCustomersSpec : BaseSpecification<Service>
    {
        public ServiceByIdWithCustomersSpec(int id)
            : base(s => s.Id == id)
        {
            AddInclude(s => s.customerServices);
        }
    }
}
