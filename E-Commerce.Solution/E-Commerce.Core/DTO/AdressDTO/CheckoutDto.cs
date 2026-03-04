namespace E_Commerce.Core.DTO.AdressDTO
{
    public class CheckoutDto
    {

        public Guid AddressId { get; set; }
        public AdressRequest? NewAddress { get; set; }

    }
}
