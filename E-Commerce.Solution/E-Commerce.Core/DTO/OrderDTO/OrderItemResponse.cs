namespace E_Commerce.Core.DTO.OrderDTO
{
    public class OrderItemResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal TotalPrice => UnitPrice * Quantity;
        public int Quantity { get; set; }
    }
}
