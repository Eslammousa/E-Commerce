using E_Commerce.Core.Domain.Entities;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface ICategoryRepository
    {
         Task<Category> AddCategory(Category category);

         Task<Category> UpdateCategory(Category category);

         Task<bool> DeleteCategoryById(Guid categoryId );

         Task<Category> GetCategoryById(Guid categoryId);

         Task<IEnumerable<Category>> GetAllCategories();
    }
}
