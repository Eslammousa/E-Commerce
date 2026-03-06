using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;


    }
}
