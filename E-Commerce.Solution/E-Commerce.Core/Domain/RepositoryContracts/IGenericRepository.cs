using System.Linq.Expressions;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T Object);

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includes);

        Task<T?> FindAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includes);

        Task<T>UpdateAsync(T Object);

        Task<bool> DeleteByIdAsync(Guid id);


    }
}
