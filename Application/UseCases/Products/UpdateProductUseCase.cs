using Application.Common;
using Application.DTOs.Products;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.UseCases.Products;

public class UpdateProductUseCase(IProductRepository repo, ICloudinaryService cloudinary)
{
    public async Task<Result<ProductResponseDto>> ExecuteAsync(UpdateProductDto dto)
    {
        var product = await repo.GetByIdAsync(dto.Id);
        if (product is null) return Result<ProductResponseDto>.Failure("Producto no encontrado");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Cost = dto.Cost;
        product.Stock = dto.Stock;
        product.Contenido = dto.Contenido;
        product.IsActive = dto.IsActive;
        product.Category = (Domain.Entities.ProductCategory)dto.Category;
        product.UpdatedAt = DateTime.UtcNow;

        if (dto.Image != null)
        {
            if (!string.IsNullOrEmpty(product.CloudinaryPublicId))
                await cloudinary.DeleteImageAsync(product.CloudinaryPublicId);

            var (url, publicId) = await cloudinary.UploadImageAsync(dto.Image);
            product.ImageUrl = url;
            product.CloudinaryPublicId = publicId;
        }

        await repo.UpdateAsync(product);
        await repo.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(CreateProductUseCase.MapToDto(product, isAdmin: true));
    }
}
