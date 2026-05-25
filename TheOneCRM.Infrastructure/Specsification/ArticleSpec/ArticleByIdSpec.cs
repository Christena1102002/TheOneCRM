using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ArticleSpec
{
    public class ArticleByIdSpec : BaseSpecification<Article>
    {
        public ArticleByIdSpec(int id) : base(a => a.Id == id)
        {
            AddInclude(a => a.CreatedBy!);
            AddInclude(a => a.Attachments);
        }
    }
}
