using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Articles;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ArticleSpec
{
    public class ArticlesCountSpec : BaseSpecification<Article>
    {
        public ArticlesCountSpec(ArticleParams p, string role, bool isAdmin)
            : base(ArticleVisibility.Build(p, role, isAdmin))
        {
        }
    }
}
