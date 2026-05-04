using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<T?> GetEntityWithSpec(ISpecification<T> spec);

        Task<int> CountAsync(ISpecification<T> spec);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        void RemoveRange(IEnumerable<T> entities);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<IReadOnlyList<TResult>> ListWithSelectAsync<TResult>(
      ISpecification<T> spec,
      Expression<Func<T, TResult>> selector);
    }
}
