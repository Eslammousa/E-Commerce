using E_Commerce.Core.Domain.Enums;
using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } 
        public decimal TotalAmount { get; set; }
        public StatusOrder Status { get; set; } = StatusOrder.Pending;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid AddressId { get; set; }
        public Address Address { get; set; } = null!;
        public string ShippingCountry { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingStreet { get; set; } = null!;
        public string ShippingBuilding { get; set; } = null!;


    }
}
