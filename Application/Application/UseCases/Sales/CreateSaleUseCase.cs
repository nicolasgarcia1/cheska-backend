using Application.Common;
using Application.DTOs.Sales;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.UseCases.Sales;

public class CreateSaleUseCase(ISaleRepository saleRepo, IProductRepository productRepo)
{
    public async Task<Result<SaleResponseDto>> ExecuteAsync(CreateSaleDto dto)
    {
        var sale = new Sale
        {
            CustomerName = dto.CustomerName,
            Notes = dto.Notes,
            Channel = (SaleChannel)dto.Channel,
            SaleDate = DateTime.UtcNow
        };

        foreach (var itemDto in dto.Items)
        {
            var product = await productRepo.GetByIdAsync(itemDto.ProductId);
            if (product is null)
                return Result<SaleResponseDto>.Failure($"Producto {itemDto.ProductId} no encontrado");

            if (product.Stock < itemDto.Quantity)
                return Result<SaleResponseDto>.Failure($"Stock insuficiente para {product.Name}");

            sale.Items.Add(new SaleItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price,
                UnitCost = product.Cost
            });

            // Actualizar stock automáticamente
            product.Stock -= itemDto.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await productRepo.UpdateAsync(product);
        }

        sale.TotalAmount = sale.Items.Sum(i => i.Quantity * i.UnitPrice);

        await saleRepo.AddAsync(sale);
        await saleRepo.SaveChangesAsync();

        return Result<SaleResponseDto>.Success(MapToDto(sale));
    }

    public static SaleResponseDto MapToDto(Sale s) => new()
    {
        Id = s.Id,
        SaleDate = s.SaleDate,
        CustomerName = s.CustomerName,
        Notes = s.Notes,
        Channel = s.Channel.ToString(),
        TotalAmount = s.TotalAmount,
        TotalProfit = s.Items.Sum(i => i.Profit),
        Items = s.Items.Select(i => new SaleItemResponseDto
        {
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? "",
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Subtotal = i.Subtotal
        }).ToList()
    };
}