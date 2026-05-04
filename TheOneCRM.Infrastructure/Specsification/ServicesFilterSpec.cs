using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.ServicesDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class ServicesFilterSpec : BaseSpecification<Service>
    {
        public ServicesFilterSpec(ServiceQueryParams p)
            : base(s =>
                string.IsNullOrEmpty(p.Search) ||
                s.NameAr.Contains(p.Search) ||
                s.NameEn.Contains(p.Search))
        {
            ApplyOrderBy(s => s.NameAr);
            ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);
        }
    }
}
