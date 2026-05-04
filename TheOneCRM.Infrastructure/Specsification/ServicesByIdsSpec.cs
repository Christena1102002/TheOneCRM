using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class ServicesByIdsSpec:BaseSpecification<Service>
    {
        public ServicesByIdsSpec(IEnumerable<int> ids)
       : base(s => ids.Contains(s.Id))
        {
        }
        public ServicesByIdsSpec(int id)
          : base(s => s.Id == id)
        {
        }
    }
}
