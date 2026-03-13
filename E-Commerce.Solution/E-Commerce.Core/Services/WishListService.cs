using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.WishlistDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace E_Commerce.Core.Services
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public WishListService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<WishlistResponse> AddProductInWishList(WishlistAddRequest request)
        {
            var userId = _currentUserService.UserId;

            if (request.ProductId == Guid.Empty) throw new InvalidIdException("Product Id is required");

            var product = await _unitOfWork.Products.FindAsync(x => x.Id == request.ProductId);

            if (product == null) throw new EntityNotFoundException("Product not found");

            var wishlist = await _unitOfWork.WishLists.FindAsync(w => w.ProductId == request.ProductId && w.UserId == userId);

            if (wishlist != null) throw new InvalidOperationException("Product is already in the wishlist");

            var wishListEntity = new WishList
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = request.ProductId,
            };

            await _unitOfWork.WishLists.AddAsync(wishListEntity);
            await _unitOfWork.SaveAsync();
            wishListEntity.product = product;

            return _mapper.Map<WishlistResponse>(wishListEntity);

        }

        public async Task<bool> ClearWishListAsync()
        {
            var userId = _currentUserService.UserId;
            var wishlists =await _unitOfWork.WishLists.WhereAsync(w => w.UserId == userId);
            if (wishlists.Items == null || !wishlists.Items.Any()) throw new EntityNotFoundException("Wishlist is already empty");
            await _unitOfWork.WishLists.DeleteRangeAsync(wishlists.Items);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteProductFromWishList(WishlistAddRequest request)
        {
            if (request.ProductId == Guid.Empty) throw new InvalidIdException("Product Id is required");

            var userId = _currentUserService.UserId;
            var wishlist = await _unitOfWork.WishLists.FindAsync(w => w.ProductId == request.ProductId && w.UserId == userId);

            if (wishlist == null) throw new EntityNotFoundException("Product not found in the wishlist");

           await _unitOfWork.WishLists.DeleteAsync(wishlist);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<WishlistResponse>> GetProductsInWishList()
        {
            var userId = _currentUserService.UserId;
            var wishlists =await _unitOfWork.WishLists.WhereAsync(w => w.UserId == userId, include: "product");

            return _mapper.Map<IEnumerable<WishlistResponse>>(wishlists.Items ?? new List<WishList>()); 

        }
    }
}
