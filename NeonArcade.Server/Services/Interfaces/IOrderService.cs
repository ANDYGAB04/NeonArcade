using NeonArcade.Server.Models;

namespace NeonArcade.Server.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(int orderId, Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<Order> CreateOrderFromCartAsync(string userId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);


    }
}
