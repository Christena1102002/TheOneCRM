using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Infrastructure.Specsification.CampaignsSpec
{
    public class CampaignPaginationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 4; // الصفحة فيها 4 

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // الفلترتين
        public int? ChannelSourceId { get; set; }   // فلترة بالمنصة
        public int? Status { get; set; } // فلترة بالحالة

        // البحث
        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }

        
    }
}
