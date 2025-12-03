using NeonArcade.Server.Models;
using NeonArcade.Server.Repositories.Interfaces;
using NeonArcade.Server.Services.Interfaces;

namespace NeonArcade.Server.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _unitOfWork.Orders.GetByIdAsync(orderId);
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _unitOfWork.Orders.GetOrderWithItemsAsync(orderId);
        }
        public async Task<Order> CreateOrderAsync(Order order) {

            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            if (string.IsNullOrEmpty(order.UserId))
            {
                throw new ArgumentException("User ID is required", nameof(order.UserId));
            }
            if (order.OrderItems == null || order.OrderItems.Count == 0)
            {
                throw new ArgumentException("Order must have at least one item", nameof(order.OrderItems));
            }
            if (order.TotalAmount <= 0)
            {
                throw new ArgumentException("Total amount must be greater than zero", nameof(order.TotalAmount));
            }
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";
            if (string.IsNullOrEmpty(order.OrderNumber))
            {
                order.OrderNumber = GenerateOrderNumber();
            }

            foreach (var item in order.OrderItems)
            {
                if (item.Price <= 0)
                    throw new ArgumentException($"Item price must be greater than zero");
                
                if (item.Quantity <= 0)
                    throw new ArgumentException($"Item quantity must be greater than zero");
                
                
                item.SubTotal = item.Price * item.Quantity;
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.SubTotal);

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Order {OrderNumber} created for user {UserId}",order.OrderNumber, order.UserId);

            return order;
        }
        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }

        private string GenerateGameKey(int gameId)
        {
            return $"{gameId}-{Guid.NewGuid():N}".ToUpper();
        }
    }
}