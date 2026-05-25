using System;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Articles
{
    public class UpdateArticleDto
    {
        public string Title { get; set; } = null!;

        public ArticleType Type { get; set; }
        public ArticleAccessLevel AccessLevel { get; set; }

        // الفئة المختارة — النوع بيتحدد من الرول زي الإنشاء
        public int CategoryId { get; set; }

        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;

        public string? Steps { get; set; }
        public string? Keywords { get; set; }

        public ArticleStatus Status { get; set; }
    }
}
