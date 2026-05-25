using System;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Articles
{
    public class ArticleParams
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        // بحث في العنوان/الملخص/المحتوى/الكلمات المفتاحية
        public string? Search { get; set; }

        // فلترة بالنوع (اختياري)
        public ArticleType? Type { get; set; }

        // فلترة بالمؤلف (اختياري)
        public string? CreatedById { get; set; }
    }
}
