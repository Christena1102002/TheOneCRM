using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Price
{
    public class PriceQuotationByIdSpec : BaseSpecification<PriceQuotation>
    {
        public PriceQuotationByIdSpec(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Customer);
            AddInclude("Items.Service");
        }
    }
}
