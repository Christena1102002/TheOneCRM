using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class ServiceByNameSpec : BaseSpecification<Service>
    {
        public ServiceByNameSpec(string nameAr, string nameEn)
            : base(s => s.NameAr == nameAr || s.NameEn == nameEn)
        {

        }
        public ServiceByNameSpec(string nameAr, string nameEn, int excludeId)
          : base(s => (s.NameAr == nameAr || s.NameEn == nameEn) && s.Id != excludeId)
        {
        }
    }
}
