using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Infrastructure.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class CartRepository : ICartRepositroy
    {

        private readonly ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db)
        {

            _db = db;
        }
        public async Task<Cart?> GetCartWithItemsAsync(Guid userId)
        {
            return await _db.Carts
              .Include(c => c.CartItems)
              .ThenInclude(ci => ci.Product)
              .FirstOrDefaultAsync(c => c.UserId == userId);
        }


    }
}
