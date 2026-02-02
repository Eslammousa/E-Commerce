using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.DTO.ProductDTO
{
    public class ProductAddRequest
    {

        [Required(ErrorMessage = "name can't by Empty")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Description can't by Empty")]
        public string Description { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please Add Price to Proudct")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Sock quantity Can't by Empty")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }


        [Required(ErrorMessage = "Please Select an Image")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = "Select a Category")]
        public Guid CategoryId { get; set; }



    }
}
