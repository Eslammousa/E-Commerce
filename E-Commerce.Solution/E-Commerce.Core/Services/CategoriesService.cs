using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;
using TaskManagementSystem.Core.Helpers;

namespace E_Commerce.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryResponse> AddCategory(CategoryAddRequest categoryAddRquest)
        {
            // convert CategoryAddRequest to Category 
            var categoryEntity = _mapper.Map<Category>(categoryAddRquest);
            categoryEntity.Id = Guid.NewGuid();

            var existingCategory = await _categoryRepository.GetCategoryByCategoryName(categoryEntity.Name);
            if (existingCategory != null) throw new DuplicateEntityException($"Category with name '{categoryEntity.Name}' already exists.");
            
            // call repository method to add category
            var addedCategory = await _categoryRepository.AddCategory(categoryEntity);

            // convert added Category to CategoryResponse 
            return _mapper.Map<CategoryResponse>(addedCategory);


        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategories()
        {
            var AllCategories = await _categoryRepository.GetAllCategories();

            return _mapper.Map<IEnumerable<CategoryResponse>>(AllCategories);
        }
        public async Task<CategoryResponse> GetCategoryById(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var category = await _categoryRepository.GetCategoryById(categoryId);

            if (category == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateCategory(Guid categoryId , CategoryUpdateRequest categoryUpdateRequest)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");
            //   categoryUpdateRequest.Id = categoryId;
            // convert categoryUpdateRequest to Category 
            var categoryEntity = _mapper.Map<Category>(categoryUpdateRequest);
            categoryEntity.Id = categoryId;

            // call repository method to add category
            var updateCategory = await _categoryRepository.UpdateCategory(categoryEntity);

            if (updateCategory is null) throw new EntityNotFoundException($"Category with id {updateCategory} not found");
            // convert added Category to CategoryResponse 
            return _mapper.Map<CategoryResponse>(updateCategory);
        }

        public async Task<bool> DeleteCategoryById(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var result = await _categoryRepository.DeleteCategoryById(categoryId);

            if (!result) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            return result;
        }
    }
}
