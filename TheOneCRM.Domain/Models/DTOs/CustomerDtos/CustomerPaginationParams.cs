using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerPaginationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string? Search { get; set; }            // اسم أو فون
        public int? SourceId { get; set; }             // ChannelSource ID
        public int? CustomerStatusId { get; set; }     // حالة العميل

    }
}
