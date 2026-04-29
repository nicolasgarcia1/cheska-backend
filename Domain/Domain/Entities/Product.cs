namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }       // Precio de venta
    public decimal Cost { get; set; }        // Costo (solo admin ve esto)
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? CloudinaryPublicId { get; set; }
    public bool IsActive { get; set; } = true;
    public ProductCategory Category { get; set; } = ProductCategory.BodySplash;

    // Navegación
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    // Computed (no mapeado a DB)
    public decimal ProfitMargin => Price > 0 ? ((Price - Cost) / Price) * 100 : 0;
}