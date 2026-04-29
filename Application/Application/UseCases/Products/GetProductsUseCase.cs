using Application.Common;
using Application.DTOs.Products;
using Application.Interfaces.Repositories;

namespace Application.UseCases.Products;

public class GetProductsUseCase(IProductRepository repo)
{
    public async Task<Result<IEnumerable<ProductResponseDto>>> ExecuteAsync(bool isAdmin = false)
    {
        var products = isAdmin
            ? await repo.GetAllAsync()
            : await repo.GetActiveProductsAsync();

        var dtos = products.Select(p => CreateProductUseCase.MapToDto(p, isAdmin));
        return Result<IEnumerable<ProductResponseDto>>.Success(dtos);
    }
}