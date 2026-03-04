using E_Commerce.Core.DTO.AdressDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IAdressService
    {
       Task<List<ResponseAdress>> GetAllAdressesAsync();
       
    }
}
