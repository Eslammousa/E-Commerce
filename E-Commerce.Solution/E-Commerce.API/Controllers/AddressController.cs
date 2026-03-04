using E_Commerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddressController : ControllerBase
    {
        private readonly IAdressService adressService;
        public AddressController(IAdressService adressService)
        {
            this.adressService = adressService;
        }
         [HttpGet]
         public async Task<ActionResult> GetAllAddresses()
        {
            return Ok(await adressService.GetAllAdressesAsync());
        }

    }
}
