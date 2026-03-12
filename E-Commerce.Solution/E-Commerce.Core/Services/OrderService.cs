using AutoMapper;
using E_Commerce.Core.Common;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.Enums;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.DTO.OrderDTO;
using E_Commerce.Core.Exceptions;
using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;


        public OrderService(IUnitOfWork unitOfWork, IMapper mapper
            , ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<OrderResponse> CancelOrder(Guid orderId)
        {
            if (orderId == Guid.Empty) throw new InvalidIdException("OrderId cannot be empty.");

            var userId = _currentUser.UserId;
            if (orderId == Guid.Empty)
                throw new InvalidIdException("Invalid id");

            //   var order = await _orderRepo.GetOrderDetailsAsync(orderId, userId);
            var order = await _unitOfWork.Orders.FindAsync(x => x.Id == orderId && x.UserId == userId
            , isTracked: true, include: "OrderItems.Product");

            if (order == null)
                throw new EntityNotFoundException("Order not found");
            if (order.Status != StatusOrder.Pending)
                throw new InvalidOperationException("Can't Cancle Order");

            foreach (var item in order.OrderItems)
            {
                item.Product.StockQuantity += item.Quantity;
            }

            order.Status = StatusOrder.Cancelled;

            await _unitOfWork.SaveAsync();
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse> Checkout(CheckoutDto request)
        {
            var userId = _currentUser.UserId;
            if (userId == Guid.Empty) throw new InvalidIdException("UserId cannot be empty.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cart = await _unitOfWork.Carts.FindAsync(x => x.UserId == userId
            , include: "CartItems.Product", isTracked: true);

                if (cart == null || !cart.CartItems.Any()) throw new EntityNotFoundException("Cart is empty");

                Address? address;

                if (request.AddressId != Guid.Empty)
                {
                    address = await _unitOfWork.Addresses.FindAsync(x => x.Id == request.AddressId);

                    if (address == null || address.UserId != userId)
                        throw new InvalidOperationException("Invalid address");
                }
                else if (request.NewAddress != null)
                {

                    address = new Address
                    {
                        Id = Guid.NewGuid(),
                        Country = request.NewAddress.Country,
                        City = request.NewAddress.City,
                        Street = request.NewAddress.Street,
                        Building = request.NewAddress.Building,
                        PersonName = request.NewAddress.PersonName,
                        Phone = request.NewAddress.Phone,
                        UserId = userId
                    };

                    await _unitOfWork.Addresses.AddAsync(address);
                }
                else
                {
                    throw new InvalidOperationException("Address is required");
                }
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Status = StatusOrder.Pending,
                    AddressId = address.Id,
                    PersonName = address.PersonName,
                    Phone = address.Phone,
                    ShippingCountry = address.Country,
                    ShippingCity = address.City,
                    ShippingStreet = address.Street,
                    ShippingBuilding = address.Building,

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
                        UnitPrice = item.UnitPrice,
                         Product = item.Product
                    };

                    totalAmount += orderItem.Quantity * orderItem.UnitPrice;

                    order.OrderItems.Add(orderItem);

                    item.Product.StockQuantity -= item.Quantity;

                }

                order.TotalAmount = totalAmount;

                await _unitOfWork.Orders.AddAsync(order);

                // Clear Cart
                foreach (var item in cart.CartItems)
                {
                    await _unitOfWork.CartItems.DeleteByIdAsync(item.Id);
                }

               
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<OrderResponse>(order);

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;

            }
        }

        public async Task<PagedResult<OrderResponse>> GetAllOrders(PaginationDTO paginationDTO)
        {
            var (items, totalCount) = await _unitOfWork.Orders
        .GetAllAsync(
            sortBy: paginationDTO.SortBy,
            sortDirection: paginationDTO.sortDirection,
            pageNumber: paginationDTO.Page,
            pageSize: paginationDTO.Size,
            include: "OrderItems.Product");

            if (!items.Any())
                throw new EntityNotFoundException("Order  Not found");

            return new PagedResult<OrderResponse>
            {
                Items = _mapper.Map<IEnumerable<OrderResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };

        }

        public async Task<PagedResult<OrderResponse>> GetOrdersByUserId(PaginationDTO paginationDTO)
        {
            var userId = _currentUser.UserId;
            if (userId == Guid.Empty) throw new InvalidIdException("OrderId cannot be empty.");

            var (items, totalCount) = await _unitOfWork.Orders
            .WhereAsync(
            predicate: o => o.UserId == userId,
            sortBy: paginationDTO.SortBy,
            sortDirection: paginationDTO.sortDirection,
            pageNumber: paginationDTO.Page,
            pageSize: paginationDTO.Size,
            include: "OrderItems.Product");

            if (!items.Any())
                throw new EntityNotFoundException("No Order found");

            return new PagedResult<OrderResponse>
            {
                Items = _mapper.Map<IEnumerable<OrderResponse>>(items),
                TotalCount = totalCount,
                PageNumber = paginationDTO.Page,
                PageSize = paginationDTO.Size,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationDTO.Size)
            };
        }

        public async Task<OrderResponse> GetOrderDetails(Guid orderId)
        {
            var userId = _currentUser.UserId;
            if (userId == Guid.Empty || orderId == Guid.Empty)
                throw new InvalidIdException("Invalid id");

            var order = await _unitOfWork.Orders.FindAsync(x => x.UserId == userId
             , include: "OrderItems.Product");

            if (order == null || !order.OrderItems.Any())
                throw new EntityNotFoundException("Order not found");

            return _mapper.Map<OrderResponse>(order);

        }

        public async Task<OrderResponse> UpdateOrderStatus(Guid orderId, StatusOrder statusOrder)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Invalid id");

            var order = await _unitOfWork.Orders.FindAsync(x => x.Id == orderId, isTracked: true
             , include: "OrderItems.Product");

            if (order == null)
                throw new EntityNotFoundException("Order not found");

            if (order.Status == StatusOrder.Cancelled ||
                order.Status == StatusOrder.Delivered)
                throw new InvalidOperationException("Cannot update this order");

            if (order.Status != StatusOrder.Cancelled &&
                statusOrder == StatusOrder.Cancelled)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Product.StockQuantity += item.Quantity;
                }

                order.Status = StatusOrder.Cancelled;
            }
            else if (order.Status == StatusOrder.Pending &&
                     statusOrder == StatusOrder.Shipped)
            {
                order.Status = StatusOrder.Shipped;
            }
            else if (order.Status == StatusOrder.Shipped &&
                     statusOrder == StatusOrder.Delivered)
            {
                order.Status = StatusOrder.Delivered;
            }
            else
            {
                throw new InvalidOperationException("Invalid status transition");
            }

            await _unitOfWork.SaveAsync();

            return _mapper.Map<OrderResponse>(order);
        }
    }
}