using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Infrastructure.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace E_Commerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        // ──────────────────────────────────────────────
        //  HELPER
        // ──────────────────────────────────────────────

        /// <summary>
        /// Builds a base IQueryable.
        /// isTracked = false by default → AsNoTracking for all read queries.
        /// Pass isTracked: true only when you intend to update the entity.
        /// </summary>
        private IQueryable<T> BuildQuery(bool isTracked = false, string include = "", bool ignoreFilters = false)
        {
            IQueryable<T> query = ignoreFilters
                ? _dbSet.IgnoreQueryFilters()
                : _dbSet;

            if (!isTracked)
                query = query.AsNoTracking();

            foreach (var nav in include.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(nav.Trim());

            return query;
        }

        // ──────────────────────────────────────────────
        //  ADD
        // ──────────────────────────────────────────────

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        // ──────────────────────────────────────────────
        //  UPDATE
        // ──────────────────────────────────────────────

        public Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        // ──────────────────────────────────────────────
        //  DELETE
        // ──────────────────────────────────────────────

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null) return false;
            _dbSet.Remove(entity);
            return true;
        }

        public Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public async Task<int> DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _dbSet.Where(predicate).ToListAsync();
            if (!entities.Any()) return 0;
            _dbSet.RemoveRange(entities);
            return entities.Count;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        // ──────────────────────────────────────────────
        //  FIND (single)
        // ──────────────────────────────────────────────

        /// <summary>
        /// Returns first match. Pass isTracked: true if you plan to update the result.
        /// </summary>
        public async Task<T?> FindAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = false, string include = "",
         bool ignoreFilters = false)
        {
            var query = BuildQuery(isTracked, include, ignoreFilters);
            return filter is null
                ? await query.FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync(filter);
        }

        /// <summary>Fastest single-entity lookup by PK (no includes).</summary>
        public async Task<T?> FindByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<T?> FindLastAsync<TKey>(Expression<Func<T, TKey>> orderBy, Expression<Func<T, bool>>? filter = null,
        string include = "")
        {
            var query = BuildQuery(include: include);   // isTracked = false ✅
            if (filter is not null) query = query.Where(filter);
            return await query.OrderByDescending(orderBy).FirstOrDefaultAsync();
        }

        // ──────────────────────────────────────────────
        //  GET ALL
        // ──────────────────────────────────────────────

        /// <summary>
        /// Returns all entities (optionally sorted & paged).
        /// TotalCount = full un-paged count.
        /// </summary>
        public async Task<(IEnumerable<T> Items, int TotalCount)> GetAllAsync(
            string? sortBy = null,
            string sortDirection = "asc",
            int? pageNumber = null,
            int? pageSize = null,
            string include = "",
            bool ignoreFilters = false)
        {
            var query = BuildQuery(include: include, ignoreFilters: ignoreFilters);

            if (!string.IsNullOrWhiteSpace(sortBy))
                query = query.OrderBy($"{sortBy} {sortDirection}");

            var total = await query.CountAsync();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var pn = Math.Max(pageNumber.Value, 1);
                var ps = Math.Max(pageSize.Value, 1);
                query = query.Skip((pn - 1) * ps).Take(ps);
            }

            return (await query.ToListAsync(), total);
        }

        // ──────────────────────────────────────────────
        //  WHERE
        // ──────────────────────────────────────────────

        public async Task<(IEnumerable<T> Items, int TotalCount)> WhereAsync(Expression<Func<T, bool>> predicate,
               string? sortBy = null,
               string sortDirection = "asc",
               int? pageNumber = null,
               int? pageSize = null,
               string include = "",
               bool ignoreFilters = false)
        {
            var query = BuildQuery(include: include, ignoreFilters: ignoreFilters)
                            .Where(predicate);

            if (!string.IsNullOrWhiteSpace(sortBy))
                query = query.OrderBy($"{sortBy} {sortDirection}");

            var total = await query.CountAsync();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                var pn = Math.Max(pageNumber.Value, 1);
                var ps = Math.Max(pageSize.Value, 1);
                query = query.Skip((pn - 1) * ps).Take(ps);
            }

            return (await query.ToListAsync(), total);
        }

        // ──────────────────────────────────────────────
        //  SELECT (projection)
        // ──────────────────────────────────────────────

        /// <summary>Projects to TResult — avoids loading full entity graph.</summary>
        public async Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            string include = "",
            bool ignoreFilters = false)
        {
            var query = BuildQuery(include: include, ignoreFilters: ignoreFilters);
            if (filter is not null) query = query.Where(filter);
            return await query.Select(selector).ToListAsync();
        }

        // ──────────────────────────────────────────────
        //  COUNT / EXISTS
        // ──────────────────────────────────────────────

        public async Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            bool ignoreFilters = false)
        {
            IQueryable<T> query = ignoreFilters ? _dbSet.IgnoreQueryFilters() : _dbSet;
            return predicate is null
                ? await query.CountAsync()
                : await query.CountAsync(predicate);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            bool ignoreFilters = false)
        {
            IQueryable<T> query = ignoreFilters ? _dbSet.IgnoreQueryFilters() : _dbSet;
            return await query.AnyAsync(predicate);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
            => _dbSet.AllAsync(predicate);

        // ──────────────────────────────────────────────
        //  AGGREGATE
        // ──────────────────────────────────────────────

        public Task<TResult?> MaxAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> q = filter is null ? _dbSet : _dbSet.Where(filter);
            return q.MaxAsync(selector);
        }

        public Task<TResult?> MinAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> q = filter is null ? _dbSet : _dbSet.Where(filter);
            return q.MinAsync(selector);
        }

        public Task<decimal> SumAsync(
            Expression<Func<T, decimal>> selector,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> q = filter is null ? _dbSet : _dbSet.Where(filter);
            return q.SumAsync(selector);
        }

        // ──────────────────────────────────────────────
        //  RAW SQL
        // ──────────────────────────────────────────────

        public IQueryable<T> FromSqlRaw(string sql, params object[] parameters)
            => _dbSet.FromSqlRaw(sql, parameters);

        // ──────────────────────────────────────────────
        //  TRANSACTION
        // ──────────────────────────────────────────────

        public Task<IDbContextTransaction> BeginTransactionAsync()
            => _db.Database.BeginTransactionAsync();

        // ──────────────────────────────────────────────
        //  SAVE
        // ──────────────────────────────────────────────

        public Task<int> SaveChangesAsync()
            => _db.SaveChangesAsync();
    }
}