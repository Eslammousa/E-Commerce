using AutoMapper;
using E_Commerce.Core.Common;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.CategoryDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CategoriesService(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }


        public async Task<CategoryResponse> AddCategory(CategoryAddRequest categoryAddRquest)
        {
            var categoryEntity = _mapper.Map<Category>(categoryAddRquest);
            categoryEntity.Id = Guid.NewGuid();

            var existingCategory = await _unitOfWork.Categories.FindAsync(x => x.Name == categoryEntity.Name);
            if (existingCategory != null) throw new DuplicateEntityException($"Category'{categoryEntity.Name}' already exists.");


            categoryEntity.Image = await _imageService.SaveImageAsync(categoryAddRquest.Image, "categories");

            var addedCategory = await _unitOfWork.Categories.AddAsync(categoryEntity);

            await _unitOfWork.SaveAsync();
            return _mapper.Map<CategoryResponse>(addedCategory);


        }
        public async Task<PagedResult<CategoryResponse>> GetAllCategories(PaginationDTO paginationDTO)
        {
            var (items, totalCount) = await _unitOfWork.Categories
                 .GetAllAsync(
                     sortBy: paginationDTO.SortBy,
                     sortDirection: paginationDTO.sortDirection,
                     pageNumber: paginationDTO.Page,
                     pageSize: paginationDTO.Size);

            return new PagedResult<CategoryResponse>
            {
                Items = _mapper.Map<IEnumerable<CategoryResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };
        }

        public async Task<CategoryResponse> GetCategoryById(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var category = await _unitOfWork.Categories.FindAsync(x => x.Id == categoryId);

            if (category == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateCategory(Guid categoryId, CategoryUpdateRequest categoryUpdateRequest)
        {

            var existingCategory = await _unitOfWork.Categories.FindAsync(x => x.Id == categoryId, isTracked: true);

            if (existingCategory == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            var existingCategoryByName = await _unitOfWork.Categories.FindAsync(x => x.Name == categoryUpdateRequest.Name);

            if (existingCategoryByName != null && existingCategoryByName.Id != categoryId)
                throw new DuplicateEntityException($"Category '{categoryUpdateRequest.Name}' already exists.");

            var newImage = await _imageService.UpdateImageAsync(categoryUpdateRequest.Image, existingCategory.Image, "categories");

            var category = _mapper.Map(categoryUpdateRequest, existingCategory);
            category.Image = newImage;

            var UpdateCategory = await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CategoryResponse>(UpdateCategory);
        }
        public async Task<bool> DeleteCategoryById(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var category = await _unitOfWork.Categories.FindAsync(x => x.Id == categoryId);

            if (category == null) throw new EntityNotFoundException($"category with id {categoryId} not found");

            await _imageService.DeleteImageAsync(category.Image);
            var result = await _unitOfWork.Categories.DeleteByIdAsync(categoryId);


            await _unitOfWork.SaveAsync();

            if (!result) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            return result;
        }

    }

}