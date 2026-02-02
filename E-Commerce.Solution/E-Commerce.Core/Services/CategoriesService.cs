using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class CategoriesService : ICategoriesService  
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<CategoryResponse> AddCategory(CategoryAddRequest categoryAddRquest)
        {
            // convert CategoryAddRequest to Category 
            var categoryEntity = _mapper.Map<Category>(categoryAddRquest);
            categoryEntity.Id = Guid.NewGuid();

            var existingCategory = await _unitOfWork.Categories.FindAsync(x=>x.Name == categoryEntity.Name);
            if (existingCategory != null) throw new DuplicateEntityException($"Category'{categoryEntity.Name}' already exists.");

            // call repository method to add category
            var addedCategory = await _unitOfWork.Categories.AddAsync(categoryEntity);

            await _unitOfWork.SaveAsync();
            // convert added Category to CategoryResponse 
            return _mapper.Map<CategoryResponse>(addedCategory);


        }
        public async Task<IEnumerable<CategoryResponse>> GetAllCategories()
        {
            return _mapper.Map<IEnumerable<CategoryResponse>>(await _unitOfWork.Categories.GetAllAsync());
        }
        public async Task<CategoryResponse> GetCategoryById(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var category = await _unitOfWork.Categories.FindAsync(x=>x.Id == categoryId);

            if (category == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateCategory(Guid categoryId, CategoryUpdateRequest categoryUpdateRequest)
        {

            var existingCategory = await _unitOfWork.Categories.FindAsync(x=>x.Id == categoryId);

            if (existingCategory == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            var existingCategoryByName = await _unitOfWork.Categories.FindAsync(x=>x.Name == categoryUpdateRequest.Name);

            if (existingCategoryByName != null && existingCategoryByName.Id != categoryId)
                throw new DuplicateEntityException($"Category '{categoryUpdateRequest.Name}' already exists.");

            existingCategory.Name = categoryUpdateRequest.Name;
            existingCategory.Description = categoryUpdateRequest.Description;

            var updateCategory = await _unitOfWork.Categories.UpdateAsync(existingCategory);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<CategoryResponse>(updateCategory);
        }
        public async Task<bool> DeleteCategoryById(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var result = await _unitOfWork.Categories.DeleteByIdAsync(categoryId);

            await _unitOfWork.SaveAsync();

            if (!result) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            return result;
        }

      
    }
}
