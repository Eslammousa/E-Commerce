using E_Commerce.Core.Common;
using E_Commerce.Core.DTO;
using E_Commerce.Core.DTO.AdressDTO;
using E_Commerce.Core.DTO.ProductDTO;
using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddressController : ControllerBase
    {
        private readonly IAdressService _adressService;
        public AddressController(IAdressService adressService)
        {
            _adressService = adressService;
        }
         [HttpGet]
         public async Task<ActionResult> GetAllAddresses([FromQuery] PaginationDTO paginationDTO)
        {
            var result = await _adressService.GetAllAdressesAsync(paginationDTO);
           
            var response = new ApiResponse<PagedResult<ResponseAdress>>
            {
                Success = true,
                Message = "Adresses retrieved successfully",
                Data = result
            };

            return Ok(response);
        }

    }
}
