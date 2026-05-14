using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos
{
    public class PaginatedPriceQuotationsDto
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<PriceQuotationListDto> Data { get; set; } = new();
    }
}
