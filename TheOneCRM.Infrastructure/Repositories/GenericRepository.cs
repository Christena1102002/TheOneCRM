using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Specifications;
using TheOneCRM.Infrastructure.Data;
using TheOneCRM.Infrastructure.Specsification;

namespace TheOneCRM.Infrastructure.Migrations
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        //protected readonly DbSet<T> _dbSet;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            var query = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            var query = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.CountAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(predicate)
                .ExecuteDeleteAsync(ct);
        }

    }
}
