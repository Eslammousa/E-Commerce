using E_Commerce.Core.Domain.Enums;
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
        public async Task<ActionResult> CheckOut()
        {

            var result = await _orderService.Checkout();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {

            var orders = await _orderService.GetOrderById();
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult> GetOrderDetails(Guid orderId)
        {


            var result = await _orderService.GetOrderDetails(orderId);

            return Ok(result);
        }


        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult> CancleOrder(Guid orderId)
        {
            return Ok(await _orderService.CancelOrder(orderId));
        }

        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{OrderId:guid}")]
        public async Task<ActionResult> UpdateOrder(Guid orderId , [FromBody] UpdateOrderStatusRequest statusOrder)
        {
            await _orderService.UpdateOrderStatus(orderId, statusOrder.Status);
            return NoContent();
        }

    }

}