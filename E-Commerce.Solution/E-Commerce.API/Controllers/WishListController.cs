using E_Commerce.Core.DTO.WishlistDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductInWishList([FromBody] WishlistAddRequest request)
        {
            var result = await _wishListService.AddProductInWishList(request);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProductFromWishList([FromBody] WishlistAddRequest request)
        {
            var result = await _wishListService.DeleteProductFromWishList(request);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearWishList()
        {
            var result = await _wishListService.ClearWishListAsync();
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            var result = await _wishListService.GetProductsInWishList();
            return Ok(result);
        }

    }
}