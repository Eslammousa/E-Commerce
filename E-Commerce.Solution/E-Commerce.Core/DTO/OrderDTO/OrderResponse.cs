using E_Commerce.Core.DTO.AdressDTO;

namespace E_Commerce.Core.DTO.OrderDTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;

        public ResponseAdress ShippingAddress { get; set; } = new();
        public List<OrderItemResponse> OrderItems { get; set; } = new();

        public decimal TotalPrice { get; set; }

    }
}
