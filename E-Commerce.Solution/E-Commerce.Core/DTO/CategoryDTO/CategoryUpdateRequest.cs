using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.DTO.CategoryDTO
{
    public class CategoryUpdateRequest
    {

        [Required(ErrorMessage = "Category Name Can't by Empty"), MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description Name Can't by Empty"), MaxLength(500)]
        public string Description { get; set; } = string.Empty;

    }
}
