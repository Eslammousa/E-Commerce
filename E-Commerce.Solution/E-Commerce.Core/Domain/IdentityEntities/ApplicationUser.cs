using E_Commerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

        public string UserType { get; set; } = "User";

        public string? RefreshToken { get; set; } // to store refresh token

        public DateTime? RefreshTokenExpirationDateTime { get; set; }

        public Cart Carts { get; set; } = null!;

       public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
