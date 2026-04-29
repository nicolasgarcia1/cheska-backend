using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProductRepository(AppDbContext context)
    : BaseRepository<Product>(context), IProductRepository
{
    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        => await _dbSet.Where(p => p.IsActive && !p.IsDeleted)
                       .OrderBy(p => p.Name)
                       .ToListAsync();

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 5)
        => await _dbSet.Where(p => !p.IsDeleted && p.Stock <= threshold && p.IsActive)
                       .OrderBy(p => p.Stock)
                       .ToListAsync();

    public async Task<bool> ExistsAsync(int id)
        => await _dbSet.AnyAsync(p => p.Id == id && !p.IsDeleted);
}