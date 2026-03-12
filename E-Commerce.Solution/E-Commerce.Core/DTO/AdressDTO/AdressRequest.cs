using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.DTO.AdressDTO
{
    public class AdressRequest
    {

        [Required(ErrorMessage ="Please add person name")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Should contain only numbers")]
        [DataType(DataType.PhoneNumber)]
        public string Phone {  get; set; } = string.Empty ;

        [Required(ErrorMessage = "please add Street")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "please add the Street")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "please add the Country")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "please add the Building")]
        public string Building { get; set; } = string.Empty;
   
    }
}
