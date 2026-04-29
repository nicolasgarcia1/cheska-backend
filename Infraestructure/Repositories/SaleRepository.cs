using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class SaleRepository(AppDbContext context)
    : BaseRepository<Sale>(context), ISaleRepository
{
    public async Task<IEnumerable<Sale>> GetSalesWithItemsAsync()
        => await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

    public async Task<IEnumerable<Sale>> GetSalesByMonthAsync(int year, int month)
        => await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .Where(s => s.SaleDate.Year == year && s.SaleDate.Month == month)
            .ToListAsync();

    public async Task<IEnumerable<object>> GetTopProductsAsync(int top = 5)
        => await _context.SaleItems
            .Include(i => i.Product)
            .GroupBy(i => new { i.ProductId, i.Product.Name })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.Name,
                TotalQuantity = g.Sum(i => i.Quantity),
                TotalRevenue = g.Sum(i => i.Quantity * i.UnitPrice)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(top)
            .Cast<object>()
            .ToListAsync();

    public async Task<IEnumerable<Sale>> GetSalesWithItemsByDateRangeAsync(DateTime from, DateTime to)
        => await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .Where(s => s.SaleDate >= from && s.SaleDate <= to)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
}