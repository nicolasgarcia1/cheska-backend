using Application.Common;
using Application.DTOs.Products;
using Application.Interfaces.Repositories;

namespace Application.UseCases.Products;

public class ReplenishProductStockUseCase(IProductRepository repo)
{
    public async Task<Result<ProductResponseDto>> ExecuteAsync(int id, ReplenishProductStockDto dto)
    {
        if (dto.Quantity <= 0)
        {
            return Result<ProductResponseDto>.Failure("La cantidad a reponer debe ser mayor a cero");
        }

        if (dto.Cost < 0)
        {
            return Result<ProductResponseDto>.Failure("El costo no puede ser negativo");
        }

        if (dto.Price is < 0)
        {
            return Result<ProductResponseDto>.Failure("El precio no puede ser negativo");
        }

        var product = await repo.GetByIdAsync(id);
        if (product is null) return Result<ProductResponseDto>.Failure("Producto no encontrado");

        product.Stock += dto.Quantity;
        product.Cost = dto.Cost;

        if (dto.Price.HasValue)
        {
            product.Price = dto.Price.Value;
        }

        product.IsActive = true;
        product.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(product);
        await repo.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(CreateProductUseCase.MapToDto(product, isAdmin: true));
    }
}
