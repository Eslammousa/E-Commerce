using E_Commerce.Core.Services;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService )
        {
            _orderService = orderService;
            
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut()
        {
          
            var result = await _orderService.Checkout();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
          
            var orders = await _orderService.GetOrderById();
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderDetails(Guid orderId)
        {
          

            var result = await _orderService.GetOrderDetails(orderId);

            return Ok(result);
        }
    }
}
