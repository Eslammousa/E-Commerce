using E_Commerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ConfigReview : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Comment)
                    .IsRequired()
                    .HasMaxLength(1000);

            builder.Property(r => r.Rating)
                    .IsRequired();

            builder.Property(r => r.CreatedAt)
       .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(r => new { r.UserId, r.ProductId })
          .IsUnique();
            builder.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
    
                builder.HasOne(r => r.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
