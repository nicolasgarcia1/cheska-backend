namespace Application.DTOs.Sales;

public class CreateSaleDto
{
    public string? CustomerName { get; set; }
    public string? Notes { get; set; }
    public int Channel { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}