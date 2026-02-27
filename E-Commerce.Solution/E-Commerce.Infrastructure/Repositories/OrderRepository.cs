using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Infrastructure.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Order>> GetOrderByUserIdAsync(Guid userId)
        {
            return await _db.Orders
           .Where(o => o.UserId == userId)
           .Include(o => o.OrderItems)
           .ThenInclude(oi => oi.Product)
           .OrderByDescending(o => o.CreatedAt)
           .ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsAsync(Guid orderId, Guid userId)
        {
            return await _db.Orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();
        }
    }
}
