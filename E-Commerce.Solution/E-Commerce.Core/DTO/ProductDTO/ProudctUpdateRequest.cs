using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.DTO.ProductDTO
{
    public class ProudctUpdateRequest
    {
   
        [Required(ErrorMessage = "Description can't by Empty")]
        public string Name { get; set; } = null!;
        

        [Required(ErrorMessage = "Please Add Price to Proudct")]
        public string Description { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please Add Price to Proudct")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Sock quantity Can't by Empty")]
        public int StockQuantity { get; set; }


        [Required(ErrorMessage = "Please Select an Image")]
        public IFormFile Image { get; set; } = null!;



        [Required(ErrorMessage = "Select a Category")]
        public Guid CategoryId { get; set; }
    }
}
