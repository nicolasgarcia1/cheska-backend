using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Products;


public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public int Stock { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public int Category { get; set; }
    public bool IsActive { get; set; }
    public IFormFile? Image { get; set; }
}
