using E_Commerce.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Core.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }

        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string PersonName { get; set; } = string.Empty;
        public string Phone { get; set; } = null!;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
