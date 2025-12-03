using NeonArcade.Server.Models;

namespace NeonArcade.Server.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<Order?> GetOrderByNumberAsync(string orderNumber);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10);
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetRevenueByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<Order> AddAsync(Order order);
    }
}
