using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.DTO.OrderDTO
{
    public class UserInFormation
    {
        [Required(ErrorMessage = "PersonName is required")]
        public string PersonName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Should contain only numbers")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;


        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;
    }
}
