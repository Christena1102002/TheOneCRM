using System;
using System.Collections.Generic;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Articles
{
    public class ArticleResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public ArticleType Type { get; set; }
        public string TypeName { get; set; } = null!;

        public ArticleAccessLevel AccessLevel { get; set; }
        public string AccessLevelName { get; set; } = null!;

        public string CategoryType { get; set; } = null!;
        public int CategoryId { get; set; }

        public string Summary { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Steps { get; set; }
        public string? Keywords { get; set; }

        public ArticleStatus Status { get; set; }
        public string StatusName { get; set; } = null!;

        public string? CreatedById { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ArticleAttachmentDto> Attachments { get; set; } = new();
    }
}
