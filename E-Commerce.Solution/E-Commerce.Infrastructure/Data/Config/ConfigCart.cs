using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ConfigCart : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.User)
                   .WithOne(u => u.Carts)
                     .HasForeignKey<Cart>(c => c.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
