using E_Commerce.Core.Common;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
           
            var response = new ApiResponse<PagedResult<CategoryResponse>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<ActionResult> GetCategoryById([FromRoute] Guid categoryId)
        { 
            var result  = await _categoriesService.GetCategoryById(categoryId);

            var response = new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddCategory([FromForm]CategoryAddRequest categoryAddRequest)
        {
            var result = await _categoriesService.AddCategory(categoryAddRequest);
             
            var response = new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category added successfully",
                Data = result
            };
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = result.Id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromForm] CategoryUpdateRequest categoryUpdateRequest)
        {
            var result = await _categoriesService.UpdateCategory(categoryId ,categoryUpdateRequest);
            
            var response = new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = result
            };
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategoryById([FromRoute] Guid categoryId)
        {
            var result  = await _categoriesService.DeleteCategoryById(categoryId);
            
            var response = new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Category deleted successfully" : "Failed to delete category",
                Data = result
            };
            return Ok(response);
        }
    }
}
