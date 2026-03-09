using E_Commerce.Core.ServicesContracts;

namespace E_Commerce.Core.Domain.Entities
{
    public class Product : BaseClass , ISoftDeleteable 
    {    
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
       public string Image { get; set; } = string.Empty;

        public float AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get ; set ; }
    }
}
