namespace E_Commerce.Core.DTO.CartDTO
{
    public class CartAddRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
