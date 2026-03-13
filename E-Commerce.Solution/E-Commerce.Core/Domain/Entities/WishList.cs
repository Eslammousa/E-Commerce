using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.Entities
{
    public class WishList
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser user { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product product { get; set; } = null!;
    }
}
