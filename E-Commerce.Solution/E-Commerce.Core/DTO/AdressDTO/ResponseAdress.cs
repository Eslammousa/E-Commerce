namespace E_Commerce.Core.DTO.AdressDTO
{
    public class ResponseAdress
    {
        public Guid Id { get; set; }

        public string PersonName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
    }
}
