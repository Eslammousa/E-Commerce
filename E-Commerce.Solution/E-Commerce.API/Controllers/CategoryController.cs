using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.ServicesContracts;
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
        public async Task<ActionResult> GetAllCategories()
        {
            var result =  await _categoriesService.GetAllCategories();
            return Ok(result);
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<ActionResult> GetCategoryById(Guid categoryId)
        { 
            return Ok(await _categoriesService.GetCategoryById(categoryId));
        }


        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryAddRequest categoryAddRequest)
        {
            var result = await _categoriesService.AddCategory(categoryAddRequest);
             return CreatedAtAction(nameof(GetAllCategories), new { id = result.Id },result);
           // return Ok(result);
        }


        [HttpPut("{categoryId:guid}")]
        public async Task<ActionResult> UpdateCategory(Guid categoryId, CategoryUpdateRequest categoryUpdateRequest)
        {
            var result = await _categoriesService.UpdateCategory(categoryId ,categoryUpdateRequest);
            return NoContent();
        }

        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategoryById(Guid categoryId)
        {
            var result = await _categoriesService.DeleteCategoryById(categoryId);
            return NoContent();
        }
    }
}
