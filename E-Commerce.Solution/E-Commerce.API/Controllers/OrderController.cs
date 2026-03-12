using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.DTO.OrderDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }

        [HttpPost("checkout")]
        public async Task<ActionResult> CheckOut([FromBody] CheckoutDto request)
        {

            var result = await _orderService.Checkout(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders([FromQuery] PaginationDTO paginationDTO)
        {
            var orders = await _orderService.GetOrdersByUserId(paginationDTO);
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult> GetOrderDetails([FromRoute] Guid orderId)
        {

            var result = await _orderService.GetOrderDetails(orderId);

            return Ok(result);
        }


        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult> CancleOrder([FromRoute] Guid orderId)
        {
            return Ok(await _orderService.CancelOrder(orderId));
        }

        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllOrders([FromQuery] PaginationDTO paginationDTO)
        {
            var orders = await _orderService.GetAllOrders(paginationDTO);
            return Ok(orders);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{OrderId:guid}")]
        public async Task<ActionResult> UpdateOrder([FromRoute] Guid orderId, [FromBody] UpdateOrderStatusRequest statusOrder)
        {
            await _orderService.UpdateOrderStatus(orderId, statusOrder.Status);
            return NoContent();
        }

    }

}