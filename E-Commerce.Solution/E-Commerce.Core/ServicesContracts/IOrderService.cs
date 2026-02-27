using E_Commerce.Core.DTO.OrderDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IOrderService
    {
        public Task<OrderResponse>Checkout(Guid UserId);

        public Task<List<OrderResponse>> GetOrderById(Guid UserId);

        public Task<OrderResponse>GetOrderDetails(Guid orderId, Guid userId);

    }
}
