using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.ProductDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsService( IGenericRepository<Product> genericRepository, IImageService imageService, IMapper mapper , IUnitOfWork unitOfWork)
        {
          
            _genericRepository = genericRepository;
            _imageService = imageService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest)
        {

            var existProduct = await _genericRepository.FindAsync(x => x.Name.ToLower() == productAddRequest.Name.ToLower()!);

            if (existProduct != null) throw new DuplicateEntityException($"Proudct'{existProduct.Name}' already exists.");

            var ProudctEntity = _mapper.Map<Product>(productAddRequest);
            ProudctEntity.Id = Guid.NewGuid();

            ProudctEntity.Image = await _imageService.SaveProductImageAsync(productAddRequest.Image);


            var addedProduct = await _genericRepository.AddAsync(ProudctEntity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductResponse>(addedProduct);

        }

        public async Task<bool> DeleteProduct(Guid Id)
        {
            if (Id == Guid.Empty) throw new InvalidIdException($"Id {Id} Can't be Empty");

            var proudct = await _genericRepository.FindAsync(x => x.Id == Id);

            if (proudct == null) throw new EntityNotFoundException($"Product with id {Id} not found");

            await _imageService.DeleteProductImageAsync(proudct.Image);

            var deleted = await _genericRepository.DeleteByIdAsync(Id);
            await _unitOfWork.SaveAsync();
            if (!deleted) throw new EntityNotFoundException($"Category with id {Id} not found");

            return deleted;

        }

        public async Task<IEnumerable<ProductResponse>> GetAllProudcts()
        {
            var getAll = await _genericRepository.GetAllAsync(x => x.Category);
            return _mapper.Map<IEnumerable<ProductResponse>>(getAll);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProudctsByCategoryId(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new InvalidIdException($"Id {categoryId} Can't be Empty");

            var proudct = await _genericRepository.FindAsync(x => x.CategoryId == categoryId);

            if (proudct == null) throw new EntityNotFoundException($"Category with id {categoryId} not found");

            var result = await _genericRepository.WhereAsync(x => x.CategoryId == categoryId
            , x => x.Category);
            return _mapper.Map<IEnumerable<ProductResponse>>(result);
        }

        public async Task<ProductResponse> GetProductByProductId(Guid id)
        {
            if (id == Guid.Empty) throw new InvalidIdException($"Id {id} Can't be Empty");
            var proudct = await _genericRepository.FindAsync(x => x.Id == id, x => x.Category);

            if (proudct == null) throw new EntityNotFoundException($"Product with id {id} not found");

            return _mapper.Map<ProductResponse>(proudct);
        }

        public async Task<ProductResponse> GetProductByProductName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            var proudct = await _genericRepository.FindAsync(x => x.Name.ToLower() == name.ToLower()!);
            if (proudct == null) throw new EntityNotFoundException($"Product with name {name} not found");
            return _mapper.Map<ProductResponse>(proudct);
        }

        public async Task<IEnumerable<ProductResponse>> Search(string keyword)
        {
            var result = await _genericRepository.WhereAsync(
       p => p.Name.Contains(keyword) ||
            (p.Description != null && p.Description.Contains(keyword)),
       p => p.Category);

            return _mapper.Map<IEnumerable<ProductResponse>>(result);
        }

        public async Task<ProductResponse> UpdateProduct(Guid Id, ProudctUpdateRequest proudctUpdateRequest)
        {
            var proudct = await _genericRepository.FindAsync(x => x.Id == Id, x => x.Category);

            if (proudct == null) throw new EntityNotFoundException($"Product with id {Id} not found");

            var existingCategoryByName = await _genericRepository.FindAsync(x => x.Name == proudctUpdateRequest.Name);

            if (existingCategoryByName != null && existingCategoryByName.Id != Id)
                throw new DuplicateEntityException($"Category '{proudctUpdateRequest.Name}' already exists.");

            var NewImage = await _imageService.UpdateProductImageAsync(proudctUpdateRequest.Image, proudct.Image);

            var ProudctEntity = _mapper.Map<Product>(proudctUpdateRequest);

            ProudctEntity.Name = proudctUpdateRequest.Name;
            ProudctEntity.Description = proudctUpdateRequest.Description;
            ProudctEntity.Price = proudctUpdateRequest.Price;
            ProudctEntity.StockQuantity = proudctUpdateRequest.StockQuantity;

            var UpdateProudct = await _genericRepository.UpdateAsync(ProudctEntity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductResponse>(UpdateProudct);



        }
    }
}
