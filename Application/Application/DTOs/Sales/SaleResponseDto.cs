namespace Application.DTOs.Sales;

public class SaleResponseDto
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public string? CustomerName { get; set; }
    public string? Notes { get; set; }
    public string Channel { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal TotalProfit { get; set; }
    public List<SaleItemResponseDto> Items { get; set; } = new();
}