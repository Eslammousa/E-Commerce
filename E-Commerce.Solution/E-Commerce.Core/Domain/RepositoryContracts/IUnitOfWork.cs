using E_Commerce.Core.Domain.Entities;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }

        Task<int> SaveAsync();
    }
}
