using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISaleRepository : IBaseRepository<Sale>
{
    Task<IEnumerable<Sale>> GetSalesWithItemsAsync();
    Task<IEnumerable<Sale>> GetSalesByMonthAsync(int year, int month);
    Task<IEnumerable<object>> GetTopProductsAsync(int top = 5);
    Task<IEnumerable<Sale>> GetSalesWithItemsByDateRangeAsync(DateTime from, DateTime to);
}