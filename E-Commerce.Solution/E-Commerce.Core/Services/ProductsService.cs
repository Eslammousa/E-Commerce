using AutoMapper;
using E_Commerce.Core.Common;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.ProductDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsService(IImageService imageService
            , IMapper mapper, IUnitOfWork unitOfWork)
        {

            _imageService = imageService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest)
        {

            var existProduct = await _unitOfWork.Products.FindAsync(x => x.Name.ToLower() == productAddRequest.Name.ToLower()!);

            if (existProduct != null) throw new DuplicateEntityException($"Proudct'{existProduct.Name}' already exists.");

            var ProudctEntity = _mapper.Map<Product>(productAddRequest);
            ProudctEntity.Id = Guid.NewGuid();

            ProudctEntity.Image = await _imageService.SaveImageAsync(productAddRequest.Image, "products");


            var addedProduct = await _unitOfWork.Products.AddAsync(ProudctEntity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductResponse>(addedProduct);

        }

        public async Task<bool> DeleteProduct(Guid Id)
        {
            if (Id == Guid.Empty) throw new InvalidIdException($"Id {Id} Can't be Empty");

            var proudct = await _unitOfWork.Products.FindAsync(x => x.Id == Id, true);

            if (proudct == null) throw new EntityNotFoundException($"Product with id {Id} not found");

            proudct.IsDeleted = true;
            await _unitOfWork.SaveAsync();

            return true;

        }

        public async Task<PagedResult<ProductResponse>> GetAllProducts(PaginationDTO paginationDTO)
        {
            var (items, totalCount) = await _unitOfWork.Products
                .GetAllAsync(
                    sortBy: paginationDTO.SortBy,
                    sortDirection: paginationDTO.sortDirection,
                    pageNumber: paginationDTO.Page,
                    pageSize: paginationDTO.Size,
                    include: "Category");

            if (!items.Any())
                throw new EntityNotFoundException("No products found");

            return new PagedResult<ProductResponse>
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };
        }

        public async Task<PagedResult<ProductResponse>> GetAllProudctsByCategoryId(Guid categoryId, PaginationDTO paginationDTO)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var proudct = await _unitOfWork.Products.FindAsync(x => x.CategoryId == categoryId);

            if (proudct == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");


            var (items, totalCount) = await _unitOfWork.Products
              .WhereAsync(
                x => x.CategoryId == categoryId,
                  sortBy: paginationDTO.SortBy,
                  sortDirection: paginationDTO.sortDirection,
                  pageNumber: paginationDTO.Page,
                  pageSize: paginationDTO.Size,
                  include: "Category");
            if (!items.Any())
                throw new EntityNotFoundException("No products found");

            return new PagedResult<ProductResponse>
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };
        }


        public async Task<PagedResult<ProductResponse>> GetDeletedProducts(PaginationDTO paginationDTO)
        {
            var (items, totalCount) = await _unitOfWork.Products
                .WhereAsync(
                    predicate: x => x.IsDeleted,
                    sortBy: paginationDTO.SortBy,
                    sortDirection: paginationDTO.sortDirection,
                    pageNumber: paginationDTO.Page,
                    pageSize: paginationDTO.Size,
                    ignoreFilters: true);

            if (!items.Any())
                throw new EntityNotFoundException("No deleted products found");

            return new PagedResult<ProductResponse>
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };
        }

        public async Task<ResponseProductWithReview> GetProductByProductId(Guid id)
        {
            if (id == Guid.Empty) throw new InvalidIdException($"Id {id} Can't be Empty");

            var proudct = await _unitOfWork.Products.FindAsync(x => x.Id == id
            , include: "Reviews.User");


            if (proudct == null) throw new EntityNotFoundException($"Product with id {id} not found");

            return _mapper.Map<ResponseProductWithReview>(proudct);
        }

        public async Task<ProductResponse> GetProductByProductName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            var proudct = await _unitOfWork.Products.FindAsync(x => x.Name.ToLower() == name.ToLower()!);
            if (proudct == null) throw new EntityNotFoundException($"Product with name {name} not found");
            return _mapper.Map<ProductResponse>(proudct);
        }

        public async Task<bool> RestoreProduct(Guid Id)
        {
            if (Id == Guid.Empty) throw new InvalidIdException($"Id {Id} Can't be Empty");
            var proudct = await _unitOfWork.Products.FindAsync(x => x.Id == Id, isTracked: true, ignoreFilters: true);

            if (proudct == null) throw new EntityNotFoundException($"Product with id {Id} not found");
            proudct.IsDeleted = false;
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<PagedResult<ProductResponse>> Search(string keyword, PaginationDTO paginationDTO)
        {
            if (keyword == null) throw new ArgumentException("Keyword cannot be null.", nameof(keyword));

            var result = await _unitOfWork.Products.WhereAsync(
       p => p.Name.Contains(keyword) ||
            (p.Description != null && p.Description.Contains(keyword)),

              include: "Category");

            var (items, totalCount) = await _unitOfWork.Products
                .WhereAsync(
                    x => x.Name.Contains(keyword) || (x.Description != null && x.Description.Contains(keyword)),
                    sortBy: paginationDTO.SortBy,
                    sortDirection: paginationDTO.sortDirection,
                    pageNumber: paginationDTO.Page,
                    pageSize: paginationDTO.Size,
                    include: "Category");

            if (!items.Any()) throw new EntityNotFoundException("No products found");

            return new PagedResult<ProductResponse>
            {
                Items = _mapper.Map<IEnumerable<ProductResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };


        }

        public async Task<ProductResponse> UpdateProduct(Guid Id, ProudctUpdateRequest proudctUpdateRequest)
        {
            var product = await _unitOfWork.Products.FindAsync(x => x.Id == Id , include: "Category");

            if (product == null) throw new EntityNotFoundException($"Product with id {Id} not found");

            var existingProduct = await _unitOfWork.Products.FindAsync(x => x.Name == proudctUpdateRequest.Name);

            if (existingProduct != null && existingProduct.Id != Id)
                throw new DuplicateEntityException($"Product  '{proudctUpdateRequest.Name}' already exists.");


            var NewImage = await _imageService.UpdateImageAsync(proudctUpdateRequest.Image, product.Image, "products");

            var ProudctEntity = _mapper.Map<Product>(proudctUpdateRequest);
            ProudctEntity.Image = NewImage;

            var UpdateProudct = await _unitOfWork.Products.UpdateAsync(ProudctEntity);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductResponse>(UpdateProudct);



        }
    }
}
