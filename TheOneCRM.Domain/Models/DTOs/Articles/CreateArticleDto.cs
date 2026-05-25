using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Articles
{
    // الحقول النصية للفورم (المرفقات بتتبعت كـ IFormFile منفصلة في الـ controller)
    public class CreateArticleDto
    {
        public string Title { get; set; } = null!;

        public ArticleType Type { get; set; }
        public ArticleAccessLevel AccessLevel { get; set; }

        // الفئة المختارة من قائمة CategoryOptions (مشروع للمطوّر / عميل للدعم والمبيعات)
        // النوع بيتحدد تلقائياً من رول المستخدم — تبعتي الـ id بس
        public int CategoryId { get; set; }

        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;

        public string? Steps { get; set; }
        public string? Keywords { get; set; }

        // مسودة (افتراضي) أو منشورة
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
    }
}
