using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.CartDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] CartAddRequest request)
        {


            var result = await _cartService.AddCart(request);

            var response = new ApiResponse<CartResponse>
            {
                Success = true,
                Message = "Product added to cart successfully",
                Data = result
            };
            return Ok(response);


        }

        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
            var result = await _cartService.GetCartByUserId();

            var response = new ApiResponse<CartResponse>
            {
                Success = true,
                Message = "Cart retrieved successfully",
                Data = result
            };
            return Ok(response);

        }

        [HttpDelete("{cartItemId:guid}")]
        public async Task<ActionResult> DeleteCartItem([FromRoute] Guid cartItemId)
        {

            var result = await _cartService.RemoveFromCart(cartItemId);

            var response = new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Cart item removed successfully" : "Failed to remove cart item",
            };
            return Ok(response);

        }

        [HttpPut("{cartItemId:guid}")]
        public async Task<ActionResult> UpdateCartItem([FromRoute] Guid cartItemId, [FromBody] UpdateCartItemRequest request)
        {
            var result = await _cartService.EditCartItem(cartItemId, request.Quantity);

            var response = new ApiResponse<CartResponse>
            {
                Success = true,
                Message = "Cart item updated successfully",
                Data = result
            };
            return Ok(response);
        }
    }
}
