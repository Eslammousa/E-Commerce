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
        private readonly IGenericRepository<Cart> _cartRepo;
        private readonly ICartRepositroy _cartRepositroy;
        private readonly IGenericRepository<CartItem> _cartItemRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IGenericRepository<Cart> cartRepo, IGenericRepository<CartItem> cartItemRepo,
            IGenericRepository<Product> productRepo, IMapper mapper, IUnitOfWork unitOfWork, ICartRepositroy cartRepositroy)
        {

            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _productRepo = productRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepositroy = cartRepositroy;
        }
        public async Task<CartResponse> AddCart(Guid userId, CartAddRequest request)
        {
            //  Get Product
            var product = await _productRepo.FindAsync(p => p.Id == request.ProductId);
            if (product == null)
                throw new EntityNotFoundException($"Product with id {request.ProductId} not found");

            if (request.Quantity <= 0)
                throw new InvalidQuantityException("Quantity must be greater than zero");

            if (request.Quantity > product.StockQuantity)
                throw new InsufficientStockException("Not enough stock");

            // 2️ Get Cart with Items with Product
            // Cart  -> CartItems -> Product
            var cart = await _cartRepositroy.GetCartWithItemsAsync(userId);

            // 3️ Create Cart if not exists
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _cartRepo.AddAsync(cart);
            }

            // 4️ Check if product already in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (existingItem != null)
            {
                if (existingItem.Quantity + request.Quantity > product.StockQuantity)
                    throw new InsufficientStockException("Not enough stock");

                existingItem.Quantity += request.Quantity;

                await _cartItemRepo.UpdateAsync(existingItem);
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

                await _cartItemRepo.AddAsync(newItem);
            }

            // 5️ Save changes
            await _unitOfWork.SaveAsync();

            // 6️ Reload cart to ensure navigation properties loaded
            cart = await _cartRepositroy.GetCartWithItemsAsync(userId);


            //  Use AutoMapper
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> GetCartByUserId(Guid userId)
        {
            var cart = await _cartRepositroy.GetCartWithItemsAsync(userId);

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

        public async Task<CartResponse> EditCartItem(Guid userId, Guid cartItemId, int quantity)
        {
            if (quantity <= 0)
                throw new InvalidQuantityException("Quantity must be greater than zero");

            var cartItem = await _cartItemRepo.FindAsync(
                ci => ci.Id == cartItemId,
                ci => ci.Product,
                ci => ci.Cart
            );

            if (cartItem == null)
                throw new EntityNotFoundException($"CartItem with id {cartItemId} not found");

            if (cartItem.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You cannot modify this cart item");

            if (quantity > cartItem.Product.StockQuantity)
                throw new InsufficientStockException("Not enough stock");

            cartItem.Quantity = quantity;

            await _cartItemRepo.UpdateAsync(cartItem);
            await _unitOfWork.SaveAsync();

            var cart = await _cartRepositroy.GetCartWithItemsAsync(userId);

            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<bool> RemoveFromCart(Guid userId, Guid cartItemId)
        {
            var cartItem = await _cartItemRepo.FindAsync(
                ci => ci.Id == cartItemId,
                ci => ci.Cart
            );

            if (cartItem == null)
                throw new EntityNotFoundException($"CartItem with id {cartItemId} not found");

            if (cartItem.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You cannot delete this cart item");

            await _cartItemRepo.DeleteByIdAsync(cartItemId);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            var cart = await _cartRepositroy.GetCartWithItemsAsync(userId);
            if (cart == null)
                throw new EntityNotFoundException($"Cart for user with id {userId} not found");
            foreach (var item in cart.CartItems)
            {
                await _cartItemRepo.DeleteByIdAsync(item.Id);
            }
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
