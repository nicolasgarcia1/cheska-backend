namespace Domain.Entities;

public class Sale : BaseEntity
{
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public string? CustomerName { get; set; }
    public string? Notes { get; set; }
    public decimal TotalAmount { get; set; }
    public SaleChannel Channel { get; set; } = SaleChannel.WhatsApp;

    // Navegación
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}