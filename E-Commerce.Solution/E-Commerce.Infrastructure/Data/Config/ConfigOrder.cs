using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ConfigOrder : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.AddressId);

            builder.HasOne(x => x.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Address)
                   .WithMany(a => a.Orders)
                   .HasForeignKey(o => o.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ShippingCountry)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.ShippingCity)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.ShippingStreet)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.ShippingBuilding)
                   .HasMaxLength(50)
                   .IsRequired();


        }
    }
}
