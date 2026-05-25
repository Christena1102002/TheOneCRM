using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheOneCRM.Domain.Models.Entities
{
    public class ArticleAttachment : BaseEntity
    {
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }

        // مسار/رابط الملف بعد الرفع
        public string FileUrl { get; set; } = null!;

        // اسم الملف الأصلي
        public string? FileName { get; set; }
    }
}
