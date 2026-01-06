using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Infrastructure.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Category> AddCategory(Category category)
        {
            _db.categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryById(Guid categoryId)
        {
            var category = await GetCategoryById(categoryId);
            if (category == null) return false;
            _db.categories.Remove(category);
            int rowsDeleted = await _db.SaveChangesAsync();
            return rowsDeleted > 0;

        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _db.categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryById(Guid categoryId)
        {
            return await _db.categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        }

        public async Task<Category?> UpdateCategory(Category category)
        {
            var existingCategory = await GetCategoryById(category.Id);
            if (existingCategory == null) return null;

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _db.SaveChangesAsync();
            return existingCategory;

        }
    }
}
