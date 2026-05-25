using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Article : BaseEntity
    {
        // عنوان المقالة
        public string Title { get; set; } = null!;

        // النوع
        public ArticleType Type { get; set; }

        // مستوى الوصول
        public ArticleAccessLevel AccessLevel { get; set; }

        // الفئة المختارة من القائمة (مشروع أو عميل) — Type: "Project" أو "Customer"
        public string CategoryType { get; set; } = null!;
        public int CategoryId { get; set; }

        // الملخص
        public string Summary { get; set; } = null!;

        // المحتوى (Markdown)
        public string Content { get; set; } = null!;

        // الخطوات التفصيلية (اختياري) — كل خطوة في سطر
        public string? Steps { get; set; }

        // الكلمات المفتاحية — مفصولة بفاصلة
        public string? Keywords { get; set; }

        // مسودة / منشورة
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;

        // اللي أنشأ المقالة
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser? CreatedBy { get; set; }

        // رول منشئ المقالة وقت الإنشاء — عشان رؤية "نفس الرول يشوفوا مقالات بعض"
        public string? CreatedByRole { get; set; }

        // المرفقات
        public ICollection<ArticleAttachment> Attachments { get; set; }
            = new List<ArticleAttachment>();
    }
}
