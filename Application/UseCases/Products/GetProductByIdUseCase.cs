using Application.Common;
using Application.DTOs.Products;
using Application.Interfaces.Repositories;

namespace Application.UseCases.Products;

public class GetProductByIdUseCase(IProductRepository repo)
{
    public async Task<Result<ProductResponseDto>> ExecuteAsync(int id, bool isAdmin = false)
    {
        var product = isAdmin
            ? await repo.GetByIdAsync(id)
            : (await repo.FindAsync(p => p.Id == id && p.IsActive && !p.IsDeleted)).FirstOrDefault();

        if (product is null || product.IsDeleted)
        {
            return Result<ProductResponseDto>.Failure("Producto no encontrado");
        }

        return Result<ProductResponseDto>.Success(CreateProductUseCase.MapToDto(product, isAdmin));
    }
}
