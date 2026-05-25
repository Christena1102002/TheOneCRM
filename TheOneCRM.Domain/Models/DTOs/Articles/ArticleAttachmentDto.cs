using System;

namespace TheOneCRM.Domain.Models.DTOs.Articles
{
    public class ArticleAttachmentDto
    {
        public string FileUrl { get; set; } = null!;
        public string? FileName { get; set; }
    }
}
