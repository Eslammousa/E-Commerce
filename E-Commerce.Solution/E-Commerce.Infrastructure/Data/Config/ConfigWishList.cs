using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ConfigWishList : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            // Primary Key
            builder.HasKey(w => w.Id);

            // User relationship
            builder.HasOne(w => w.user)
                   .WithMany()
                   .HasForeignKey(w => w.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Product relationship
            builder.HasOne(w => w.product)
                   .WithMany()
                   .HasForeignKey(w => w.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint to prevent duplicate wishlist items
            builder.HasIndex(w => new { w.UserId, w.ProductId })
                   .IsUnique();
        }
    }
}
