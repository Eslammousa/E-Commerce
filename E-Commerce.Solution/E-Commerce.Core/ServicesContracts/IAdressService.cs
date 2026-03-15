using E_Commerce.Core.Common;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.AdressDTO;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IAdressService
    {
        Task<PagedResult<ResponseAdress>> GetAllAdressesAsync(PaginationDTO paginationDTO);
       
    }
}
