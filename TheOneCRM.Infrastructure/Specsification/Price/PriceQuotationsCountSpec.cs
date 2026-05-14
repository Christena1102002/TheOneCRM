using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Price
{
    public class PriceQuotationsCountSpec : BaseSpecification<PriceQuotation>
    {
        public PriceQuotationsCountSpec(PriceQuotationParams p)
            : base(x =>
                (!p.CustomerId.HasValue || x.CustomerId == p.CustomerId.Value) &&
                (string.IsNullOrEmpty(p.Search)
                    || (x.Notes != null && x.Notes.Contains(p.Search))
                    || x.Customer.FullName.Contains(p.Search)))
        {
        }
    }
}
