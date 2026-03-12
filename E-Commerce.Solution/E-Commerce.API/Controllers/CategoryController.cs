using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories([FromQuery] PaginationDTO paginationDTO)
        {
            var result =  await _categoriesService.GetAllCategories(paginationDTO);
            return Ok(result);
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<ActionResult> GetCategoryById([FromRoute] Guid categoryId)
        { 
            return Ok(await _categoriesService.GetCategoryById(categoryId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddCategory([FromForm]CategoryAddRequest categoryAddRequest)
        {
            var result = await _categoriesService.AddCategory(categoryAddRequest);
             return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromForm] CategoryUpdateRequest categoryUpdateRequest)
        {
            var result = await _categoriesService.UpdateCategory(categoryId ,categoryUpdateRequest);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategoryById([FromRoute] Guid categoryId)
        {
            await _categoriesService.DeleteCategoryById(categoryId);
            return NoContent();
        }
    }
}
