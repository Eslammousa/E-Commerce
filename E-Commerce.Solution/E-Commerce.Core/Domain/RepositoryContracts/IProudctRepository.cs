using E_Commerce.Core.Domain.Entities;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface IProudctRepository
    {
        public Task<Product?> GetProudctWithReviews(Guid ProudctId);
    }
}
