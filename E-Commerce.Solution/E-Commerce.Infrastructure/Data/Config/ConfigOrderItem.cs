using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ConfigOrderItem : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                   .IsRequired();

            builder.Property(x => x.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Configure the relationship with Order
            builder.HasOne(x => x.Order)
                     .WithMany(o => o.OrderItems)
                     .HasForeignKey(oi => oi.OrderId)
                     .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship with Product
            builder.HasOne(x => x.Product)
                     .WithMany(p => p.OrderItems)
                     .HasForeignKey(oi => oi.ProductId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

