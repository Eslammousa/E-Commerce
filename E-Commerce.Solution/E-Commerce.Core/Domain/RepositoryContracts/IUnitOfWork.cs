using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Cart> Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Address> Addresses { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<WishList> WishLists { get; }


        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
