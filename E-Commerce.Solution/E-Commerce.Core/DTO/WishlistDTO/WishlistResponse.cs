namespace E_Commerce.Core.DTO.WishlistDTO
{
    public class WishlistResponse
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; } 

        public string ProductImage { get; set; } = string.Empty;



    }
}
