using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(10,2)");

        builder.Property(s => s.CustomerName)
            .HasMaxLength(200);

        builder.Property(s => s.Notes)
            .HasMaxLength(500);

        builder.HasMany(s => s.Items)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Sales");
    }
}

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(si => si.Id);

        builder.Property(si => si.UnitPrice)
            .HasColumnType("decimal(10,2)");

        builder.Property(si => si.UnitCost)
            .HasColumnType("decimal(10,2)");

        builder.Ignore(si => si.Subtotal);
        builder.Ignore(si => si.Profit);

        builder.HasOne(si => si.Product)
            .WithMany(p => p.SaleItems)
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("SaleItems");
    }
}