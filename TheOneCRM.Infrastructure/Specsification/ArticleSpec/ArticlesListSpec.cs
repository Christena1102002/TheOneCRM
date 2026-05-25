using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Articles;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ArticleSpec
{
    public class ArticlesListSpec : BaseSpecification<Article>
    {
        public ArticlesListSpec(ArticleParams p, string role, bool isAdmin)
            : base(ArticleVisibility.Build(p, role, isAdmin))
        {
            AddInclude(a => a.CreatedBy!);
            AddInclude(a => a.Attachments);
            ApplyOrderByDescending(a => a.CreatedAt);
            ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }

    // الرؤية: الأدمن يشوف الكل، وغيره يشوف:
    //  - العام (Public)
    //  - أو المقالات اللي مستوى وصولها بيخص رول المستخدم
    //  - أو المقالات اللي اتعملت من نفس الرول (نفس الرول يشوفوا مقالات بعض)
    internal static class ArticleVisibility
    {
        public static Expression<Func<Article, bool>> Build(ArticleParams p, string role, bool isAdmin)
        {
            return a =>
                (isAdmin
                    || a.AccessLevel == ArticleAccessLevel.Public
                    || a.CreatedByRole == role
                    || (a.AccessLevel == ArticleAccessLevel.DevelopersOnly && role == "Developer")
                    || (a.AccessLevel == ArticleAccessLevel.SupportOnly && role == "Support")
                    || (a.AccessLevel == ArticleAccessLevel.SalesOnly && role == "Sales")
                    || (a.AccessLevel == ArticleAccessLevel.ManagementOnly && role == "Admin"))
                && (string.IsNullOrEmpty(p.Search)
                    || a.Title.Contains(p.Search)
                    || a.Summary.Contains(p.Search)
                    || a.Content.Contains(p.Search)
                    || (a.Keywords != null && a.Keywords.Contains(p.Search)))
                && (!p.Type.HasValue || a.Type == p.Type.Value)
                && (string.IsNullOrEmpty(p.CreatedById) || a.CreatedById == p.CreatedById);
        }
    }
}
