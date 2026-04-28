using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }

        List<string> IncludeStrings { get; }
        Expression<Func<T, object>>? OrderBy { get; }

        Expression<Func<T, object>>? OrderByDescending { get; }
        List<Expression<Func<T, object>>> ThenByExpressions { get; }

        List<Expression<Func<T, object>>> ThenByDescendingExpressions { get; }

        int? Take { get; }

        int? Skip { get; }

        bool IsPagingEnabled { get; }


    }
}
