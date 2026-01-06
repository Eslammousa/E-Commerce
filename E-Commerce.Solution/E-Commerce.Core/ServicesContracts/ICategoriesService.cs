using E_Commerce.Core.DTO.CategoryDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface ICategoriesService
    {
        Task<CategoryResponse> AddCategory(CategoryAddRequest categoryAddRquest);

        Task<CategoryResponse> UpdateCategory(Guid categoryId, CategoryUpdateRequest categoryUpdateRequest);

        Task<bool> DeleteCategoryById(Guid categoryId);

        Task<CategoryResponse> GetCategoryById(Guid categoryId);

        Task<IEnumerable<CategoryResponse>> GetAllCategories();

    }
}
