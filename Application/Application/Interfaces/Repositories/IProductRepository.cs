using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 5);
    Task<bool> ExistsAsync(int id);
}