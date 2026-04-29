using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.Cost)
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        builder.Property(p => p.CloudinaryPublicId)
            .HasMaxLength(200);

        builder.Ignore(p => p.ProfitMargin); // Computed, no a DB

        builder.ToTable("Products");
    }
}