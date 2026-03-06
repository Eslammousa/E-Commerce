using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.DTO.ReviewDTO
{
    public class ReviewAddRequest
    {

        [Required(ErrorMessage = "Please add the commnet")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please add the rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; } 
    }
}
