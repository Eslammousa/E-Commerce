using E_Commerce.Core.Domain.Enums;
using E_Commerce.Core.DTO.OrderDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IOrderService
    {
        public Task<OrderResponse>Checkout();

        public Task<List<OrderResponse>> GetOrderById();

        public Task<OrderResponse>GetOrderDetails(Guid orderId);

        public Task<List<OrderResponse>> GetAllOrders();

        public Task<OrderResponse> CancelOrder(Guid orderId);

        public Task<OrderResponse> UpdateOrderStatus(Guid orderId , StatusOrder statusOrder);


    }
}
