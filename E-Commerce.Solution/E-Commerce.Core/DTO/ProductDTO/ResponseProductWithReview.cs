using E_Commerce.Core.DTO.ReviewDTO;

namespace E_Commerce.Core.DTO.ProductDTO
{
    public class ResponseProductWithReview
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Image { get; set; } = string.Empty;
        public float AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewResponse> Reviews { get; set; } 
    }
}
