using E_Commerce.Core.Common;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.ProductDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductsService productsService, ILogger<ProductController> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddProduct([FromForm] ProductAddRequest productAddRequest)
        {

            var result = await _productsService.AddProduct(productAddRequest);
            var response = new ApiResponse<ProductResponse>
            {
                Success = true,
                Message = "Products Added successfully",
                Data = result
            };
            return Ok(response);
         
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var deleted = await _productsService.DeleteProduct(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id:guid}")]
        public async Task<ActionResult> RestoreProduct([FromRoute] Guid id)
        {
            await _productsService.RestoreProduct(id);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetProductsDeleted")]
        public async Task<ActionResult> GetProductsDeleted([FromQuery] PaginationDTO paginationDTO)
        {
            var result = await _productsService.GetDeletedProducts(paginationDTO);

            var response = new ApiResponse<PagedResult<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] PaginationDTO paginationDTO)
        {
            var result = await _productsService.GetAllProducts(paginationDTO);

            var response = new ApiResponse<PagedResult<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("ProudctsByoneCategory/{CategoryId:guid}")]
        public async Task<ActionResult> GetAllProudctByCategoryId([FromRoute] Guid CategoryId, [FromQuery] PaginationDTO paginationDTO)
        {
            var result = await _productsService.GetAllProudctsByCategoryId(CategoryId, paginationDTO);

            var response = new ApiResponse<PagedResult<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("{ProudctId:guid}")]
        public async Task<ActionResult> GetProductByProudctId([FromRoute] Guid ProudctId)
        {
           var result = await _productsService.GetProductByProductId(ProudctId);
            var response = new ApiResponse<ResponseProductWithReview>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{Id:Guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid Id, [FromForm] ProudctUpdateRequest proudctUpdateRequest)
        {
            await _productsService.UpdateProduct(Id, proudctUpdateRequest);
            return NoContent();
        }

        [HttpGet("Search")]
        public async Task<ActionResult> Search([FromQuery] string keyword, [FromQuery] PaginationDTO paginationDTO)
        {
            return Ok(await _productsService.Search(keyword, paginationDTO));

        }
    }
}