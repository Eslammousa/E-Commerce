namespace E_Commerce.Core.DTO.OrderDTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShippingCountry { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingStreet { get; set; } = null!;
        public string ShippingBuilding { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
        public decimal TotalPrice => OrderItems.Sum(i => i.TotalPrice);

    }
}
