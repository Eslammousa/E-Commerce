using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.DTO.ProductDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddProduct([FromForm] ProductAddRequest productAddRequest)
        {
            var addedProduct = await _productsService.AddProduct(productAddRequest);
            // return CreatedAtAction("GetProductByProductId", new { id = addedProduct.Id }, addedProduct);

            return Ok(addedProduct);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var deleted = await _productsService.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> GetALlProudct()
        {
            return Ok( await _productsService.GetAllProudcts());
        }

        [HttpGet("ProudctsByoneCategory/{CategoryId:guid}")]
        public async Task<ActionResult>GetAllProudctByCategoryId(Guid CategoryId)
        {
            return Ok(await _productsService.GetAllProudctsByCategoryId(CategoryId)); 
        }

        [HttpGet("{ProudctId:guid}")]
        public async Task<ActionResult>GetProductByProudctId(Guid ProudctId)
        {
            return Ok(await _productsService.GetProductByProductId(ProudctId));
        }


        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Guid Id ,ProudctUpdateRequest proudctUpdateRequest)
        {
           await _productsService.UpdateProduct(Id , proudctUpdateRequest);
            return NoContent();
        }


    }
}
