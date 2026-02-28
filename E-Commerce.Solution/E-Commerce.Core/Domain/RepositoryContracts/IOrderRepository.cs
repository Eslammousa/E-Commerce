using E_Commerce.Core.Domain.Entities;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrderByUserIdAsync(Guid userId);

        Task<Order?> GetOrderDetailsAsync(Guid orderId , Guid userId);

        Task<List<Order>> GetAllOrdersAsync();
        public Task<Order?> GetOrderByIdAsync(Guid orderId);
    }
}
