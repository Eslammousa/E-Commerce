namespace E_Commerce.Core.DTO.OrderDTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
        public decimal TotalPrice => OrderItems.Sum(i => i.TotalPrice);

    }
}
