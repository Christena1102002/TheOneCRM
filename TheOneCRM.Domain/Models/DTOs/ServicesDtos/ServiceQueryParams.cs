using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.ServicesDtos
{
    public class ServiceQueryParams
    {
        public string? Search { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
