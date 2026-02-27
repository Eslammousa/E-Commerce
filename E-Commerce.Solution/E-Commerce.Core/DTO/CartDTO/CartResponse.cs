namespace E_Commerce.Core.DTO.CartDTO
{
    public class CartResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemResponse> CartItems { get; set; } = new List<CartItemResponse>();
        public decimal TotalPrice => CartItems.Sum(i => i.TotalPrice);
    }
}
