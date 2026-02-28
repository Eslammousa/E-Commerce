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
           

            var result = await _cartService.AddCart(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
           

            var result = await _cartService.GetCartByUserId();

            return Ok(result);
        }

        [HttpDelete("{cartItemId:guid}")]
        public async Task<ActionResult> DeleteCartItem(Guid cartItemId)
        {

            await _cartService.RemoveFromCart(cartItemId);

            return NoContent();
        }

        [HttpPut("{cartItemId:guid}")]
        public async Task<ActionResult> UpdateCartItem(Guid cartItemId, [FromBody] UpdateCartItemRequest request)
        {
         

            var result = await _cartService.EditCartItem(cartItemId, request.Quantity);

            return Ok(result);
        }
    }
}
