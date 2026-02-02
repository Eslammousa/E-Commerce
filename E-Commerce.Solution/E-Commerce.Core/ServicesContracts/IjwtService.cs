using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.DTO.IdentityDTO;
using System.Security.Claims;

namespace E_Commerce.Core.ServicesContracts
{
    public interface IjwtService
    {
        Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user);
        Task<ClaimsPrincipal?> GetPrincipalFromJwtToken(string? token);
    }
}

