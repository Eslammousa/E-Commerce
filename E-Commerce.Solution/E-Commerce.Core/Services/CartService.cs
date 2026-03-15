using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.CartDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;
namespace E_Commerce.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;


        public CartService(IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<CartResponse> AddCart(CartAddRequest request)
        {
            //  Get Product
            var product = await _unitOfWork.Products.FindAsync(p => p.Id == request.ProductId);
            if (product == null)
                throw new EntityNotFoundException($"Product with id {request.ProductId} not found");

            if (request.Quantity <= 0)
                throw new InvalidQuantityException("Quantity must be greater than zero");

            if (request.Quantity > product.StockQuantity)
                throw new InsufficientStockException("Not enough stock");

            // 2️ Get Cart with Items with Product
            // Cart  -> CartItems -> Product
            var userId = _currentUserService.UserId;
            var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId,
                include: "CartItems.Product");

            // 3️ Create Cart if not exists
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Carts.AddAsync(cart);
            }

            // 4️ Check if product already in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (existingItem != null)
            {
                if (existingItem.Quantity + request.Quantity > product.StockQuantity)
                    throw new InsufficientStockException("Not enough stock");

                existingItem.Quantity += request.Quantity;

                await _unitOfWork.CartItems.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price
                };

                await _unitOfWork.CartItems.AddAsync(newItem);
            }

            // 5️ Save changes
            await _unitOfWork.SaveAsync();

            // 6️ Reload cart to ensure navigation properties loaded
            //  cart = await _cartRepositroy.GetCartWithItemsAsync(userId);
            cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId, include: "CartItems.Product");


            //  Use AutoMapper
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> GetCartByUserId()
        {
            var userId = _currentUserService.UserId;
            var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId, include: "CartItems.Product");

            if (cart == null)
            {
                return new CartResponse
                {
                    Id = Guid.Empty,
                    CreatedAt = DateTime.UtcNow,
                    CartItems = new List<CartItemResponse>()
                };
            }

            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> EditCartItem(Guid cartItemId, int quantity)
        {
            var userId = _currentUserService.UserId;
            if (quantity <= 0)
                throw new InvalidQuantityException("Quantity must be greater than zero");

            var cartItem = await _unitOfWork.CartItems.FindAsync(
                        ci => ci.Id == cartItemId,
                        include: "Product,Cart");

            if (cartItem == null)
                throw new EntityNotFoundException($"CartItem with id {cartItemId} not found");

            if (cartItem.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You cannot modify this cart item");

            if (quantity > cartItem.Product.StockQuantity)
                throw new InsufficientStockException("Not enough stock");

            cartItem.Quantity = quantity;

            await _unitOfWork.CartItems.UpdateAsync(cartItem);
            await _unitOfWork.SaveAsync();

            var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId, include: "CartItems.Product");

            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<bool> RemoveFromCart(Guid cartItemId)
        {
            var cartItem = await _unitOfWork.CartItems.FindAsync(
                 ci => ci.Id == cartItemId,
                 include: "Cart");

            if (cartItem == null)
                throw new EntityNotFoundException($"CartItem with id {cartItemId} not found");

            var userId = _currentUserService.UserId;

            if (cartItem.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You cannot delete this cart item");

            await _unitOfWork.CartItems.DeleteByIdAsync(cartItemId);

            return await _unitOfWork.SaveAsync() > 0;


        }

        public async Task<bool> ClearCart()
        {
            var userId = _currentUserService.UserId;
            var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId, include: "CartItems.Product");
            if (cart == null)
                throw new EntityNotFoundException($"Cart for user with id {userId} not found");
            foreach (var item in cart.CartItems)
            {
                await _unitOfWork.CartItems.DeleteByIdAsync(item.Id);
            }
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
