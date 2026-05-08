using Application.Common;
using Application.Interfaces.Repositories;

namespace Application.UseCases.Dashboard;

public class DashboardStatsDto
{
    public decimal TotalRevenueThisMonth { get; set; }
    public decimal TotalProfitThisMonth { get; set; }
    public int TotalSalesThisMonth { get; set; }
    public List<MonthlySaleDto> SalesByMonth { get; set; } = new();
    public List<TopProductDto> TopProducts { get; set; } = new();
    public List<LowStockDto> LowStockAlerts { get; set; } = new();
}

public class MonthlySaleDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Profit { get; set; }
    public int SalesCount { get; set; }
}

public class TopProductDto
{
    public string Name { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class LowStockDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
}

public class GetDashboardStatsUseCase(ISaleRepository saleRepo, IProductRepository productRepo)
{
    public async Task<Result<DashboardStatsDto>> ExecuteAsync()
    {
        var now = DateTime.UtcNow;
        var thisMonthSales = await saleRepo.GetSalesByMonthAsync(now.Year, now.Month);
        var allSales = await saleRepo.GetSalesWithItemsAsync();
        var lowStock = await productRepo.GetLowStockProductsAsync(1);

        // Ventas por mes (últimos 6 meses)
        var salesByMonth = new List<MonthlySaleDto>();
        for (int i = 5; i >= 0; i--)
        {
            var date = now.AddMonths(-i);
            var monthlySales = await saleRepo.GetSalesByMonthAsync(date.Year, date.Month);
            var items = monthlySales.SelectMany(s => s.Items).ToList();

            salesByMonth.Add(new MonthlySaleDto
            {
                Month = date.ToString("MMM yyyy"),
                Revenue = monthlySales.Sum(s => s.TotalAmount),
                Profit = items.Sum(i => i.Profit),
                SalesCount = monthlySales.Count()
            });
        }

        // Top productos
        var topProducts = allSales
            .SelectMany(s => s.Items)
            .GroupBy(i => new { i.ProductId, Name = i.Product?.Name ?? "" })
            .Select(g => new TopProductDto
            {
                Name = g.Key.Name,
                QuantitySold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.Subtotal)
            })
            .OrderByDescending(t => t.QuantitySold)
            .Take(5)
            .ToList();

        var thisMonthItems = thisMonthSales.SelectMany(s => s.Items).ToList();

        var stats = new DashboardStatsDto
        {
            TotalRevenueThisMonth = thisMonthSales.Sum(s => s.TotalAmount),
            TotalProfitThisMonth = thisMonthItems.Sum(i => i.Profit),
            TotalSalesThisMonth = thisMonthSales.Count(),
            SalesByMonth = salesByMonth,
            TopProducts = topProducts,
            LowStockAlerts = lowStock.Select(p => new LowStockDto
            {
                Id = p.Id,
                Name = p.Name,
                Stock = p.Stock
            }).ToList()
        };

        return Result<DashboardStatsDto>.Success(stats);
    }
}
