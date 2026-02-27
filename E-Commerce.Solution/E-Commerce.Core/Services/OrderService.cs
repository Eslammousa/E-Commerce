using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.Enums;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO.OrderDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepo;
        private readonly ICartRepositroy _cartRepository;
        private readonly IOrderRepository _orderRepo;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<Order> orderRepository,
            ICartRepositroy cartRepositroy, IGenericRepository<CartItem> cartItemRepo, IOrderRepository orderRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _cartRepository = cartRepositroy;
            _cartItemRepo = cartItemRepo;
            _orderRepo = orderRepo;
        }
        public async Task<OrderResponse> Checkout(Guid userId)
        {
            if (userId == Guid.Empty) throw new InvalidIdException("UserId cannot be empty.");

            var cart = await _cartRepository.GetCartWithItemsAsync(userId);

            if (cart == null || !cart.CartItems.Any()) throw new InvalidOperationException("Cart is empty");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = StatusOrder.Pending,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in cart.CartItems)
            {
                if (item.Product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"Not enough stock for {item.Product.Name}");

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                totalAmount += orderItem.Quantity * orderItem.UnitPrice;

                order.OrderItems.Add(orderItem);

                item.Product.StockQuantity -= item.Quantity;
            }

            order.TotalAmount = totalAmount;

            await _orderRepository.AddAsync(order);

            // Clear cart manually
            foreach (var item in cart.CartItems)
            {
                await _cartItemRepo.DeleteByIdAsync(item.Id);
            }

            await _unitOfWork.SaveAsync();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<List<OrderResponse>> GetOrderById(Guid userId)
        {
           if (userId == Guid.Empty) throw new InvalidIdException("OrderId cannot be empty.");
           var order = await _orderRepo.GetOrderByUserIdAsync(userId);

            if (order == null) throw new EntityNotFoundException("Order not found.");

            return _mapper.Map<List<OrderResponse>>(order);
        }

        public async Task<OrderResponse> GetOrderDetails(Guid orderId , Guid userId)
        {
            if (userId == Guid.Empty || orderId == Guid.Empty)
                throw new ArgumentException("Invalid id");

            var order =await _orderRepo.GetOrderDetailsAsync(orderId, userId);

            if (order == null)
                throw new EntityNotFoundException("Order not found");

            return _mapper.Map<OrderResponse>(order);

        }
    }
}