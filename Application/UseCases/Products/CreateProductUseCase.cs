using Application.Common;
using Application.DTOs.Products;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.UseCases.Products;

public class CreateProductUseCase(IProductRepository repo, ICloudinaryService cloudinary)
{
    public async Task<Result<ProductResponseDto>> ExecuteAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Cost = dto.Cost,
            Stock = dto.Stock,
            Contenido = dto.Contenido,
            Category = (ProductCategory)dto.Category,
            IsActive = true
        };

        if (dto.Image != null)
        {
            var (url, publicId) = await cloudinary.UploadImageAsync(dto.Image);
            product.ImageUrl = url;
            product.CloudinaryPublicId = publicId;
        }

        await repo.AddAsync(product);
        await repo.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(MapToDto(product, isAdmin: true));
    }

    public static ProductResponseDto MapToDto(Product p, bool isAdmin = false) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Cost = isAdmin ? p.Cost : null,
        ProfitMargin = isAdmin ? p.ProfitMargin : null,
        Stock = p.Stock,
        Contenido = p.Contenido,
        ImageUrl = p.ImageUrl,
        IsActive = p.IsActive,
        Category = p.Category.ToString(),
        CreatedAt = p.CreatedAt
    };
}
