using E_Commerce.Core.DTO.CartDTO;
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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService )
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] CartAddRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _cartService.AddCart(userId, request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _cartService.GetCartByUserId(userId);

            return Ok(result);
        }

        [HttpDelete("{cartItemId:guid}")]
        public async Task<ActionResult> DeleteCartItem(Guid cartItemId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _cartService.RemoveFromCart(userId, cartItemId);

            return NoContent();
        }

        [HttpPut("{cartItemId:guid}")]
        public async Task<ActionResult> UpdateCartItem(Guid cartItemId, [FromBody] UpdateCartItemRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _cartService.EditCartItem(userId, cartItemId, request.Quantity);

            return Ok(result);
        }
    }
}
