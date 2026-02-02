using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();    
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
