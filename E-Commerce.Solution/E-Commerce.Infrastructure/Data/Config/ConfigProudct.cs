using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ConfigProudct : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                 .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired()
;

            builder.Property(p => p.Description)
              .HasColumnType("nvarchar")
                .HasMaxLength(500)
                 .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            builder.Property(p => p.StockQuantity)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.Image)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
