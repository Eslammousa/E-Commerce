using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Infrastructure.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class ProudctRepository : IProudctRepository
    {
        private readonly ApplicationDbContext _db;

        public ProudctRepository(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        public async Task<Product?> GetProudctWithReviews(Guid ProudctId)
        {
            return await _db.products
               .Include(p => p.Reviews)
               .ThenInclude(r => r.User)
               .FirstOrDefaultAsync(x => x.Id == ProudctId);
        }
    }
}
