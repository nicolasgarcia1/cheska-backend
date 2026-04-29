using Application.Common;
using Application.Interfaces.Repositories;

namespace Application.UseCases.Products;

public class DeleteProductUseCase(IProductRepository repo)
{
    public async Task<Result> ExecuteAsync(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product is null) return Result.Failure("Producto no encontrado");

        // Soft delete
        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;
        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(product);
        await repo.SaveChangesAsync();

        return Result.Success();
    }
}