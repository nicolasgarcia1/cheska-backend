namespace Domain.Entities;

public class SaleItem : BaseEntity
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }   // Precio al momento de la venta
    public decimal UnitCost { get; set; }    // Costo al momento de la venta

    public decimal Subtotal => Quantity * UnitPrice;
    public decimal Profit => Quantity * (UnitPrice - UnitCost);

    // Navegación
    public Sale Sale { get; set; } = null!;
    public Product Product { get; set; } = null!;
}