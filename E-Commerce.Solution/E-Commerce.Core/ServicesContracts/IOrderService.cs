using E_Commerce.Core.Common;
using E_Commerce.Core.Domain.Enums;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.DTO.OrderDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IOrderService
    {
         Task<OrderResponse>Checkout(CheckoutDto request);

         Task<PagedResult<OrderResponse>> GetOrdersByUserId(PaginationDTO paginationDTO);

         Task<OrderResponse>GetOrderDetails(Guid orderId);

         Task<PagedResult<OrderResponse>> GetAllOrders(PaginationDTO paginationDTO);

         Task<OrderResponse> CancelOrder(Guid orderId);

         Task<OrderResponse> UpdateOrderStatus(Guid orderId , StatusOrder statusOrder);



    }
}
