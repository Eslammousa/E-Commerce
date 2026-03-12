using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface IGenericRepository<T> where T : class
    {
        // ── ADD ──────────────────────────────────────────
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // ── UPDATE ───────────────────────────────────────
        Task<T> UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        // ── DELETE ───────────────────────────────────────
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> DeleteAsync(T entity);
        Task<int> DeleteWhereAsync(Expression<Func<T, bool>> predicate);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        // ── FIND (single) ────────────────────────────────
        Task<T?> FindAsync(
            Expression<Func<T, bool>>? filter = null,
            bool isTracked = false,
            string include = "",
            bool ignoreFilters = false);

        Task<T?> FindByIdAsync(Guid id);

        Task<T?> FindLastAsync<TKey>(
            Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, bool>>? filter = null,
            string include = "");

        // ── GET ALL / WHERE ──────────────────────────────
        // ── GET ALL / WHERE ──────────────────────────────
        Task<(IEnumerable<T> Items, int TotalCount)> GetAllAsync(
            string? sortBy = null,
            string sortDirection = "asc",
            int? pageNumber = null,
            int? pageSize = null,
            string include = "",
            bool ignoreFilters = false);

        Task<(IEnumerable<T> Items, int TotalCount)> WhereAsync(
            Expression<Func<T, bool>> predicate,
            string? sortBy = null,
            string sortDirection = "asc",
            int? pageNumber = null,
            int? pageSize = null,
            string include = "",
            bool ignoreFilters = false);

        // ── PROJECTION ───────────────────────────────────
        Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? filter = null,
            string include = "",
            bool ignoreFilters = false);

        // ── COUNT / EXISTS ───────────────────────────────
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool ignoreFilters = false);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool ignoreFilters = false);
        Task<bool> AllAsync(Expression<Func<T, bool>> predicate);

        // ── AGGREGATE ────────────────────────────────────
        Task<TResult?> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? filter = null);
        Task<TResult?> MinAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? filter = null);
        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? filter = null);

        // ── RAW SQL ──────────────────────────────────────
        IQueryable<T> FromSqlRaw(string sql, params object[] parameters);

        // ── TRANSACTION ──────────────────────────────────
        Task<IDbContextTransaction> BeginTransactionAsync();

        // ── SAVE ─────────────────────────────────────────
        Task<int> SaveChangesAsync();
    }
}