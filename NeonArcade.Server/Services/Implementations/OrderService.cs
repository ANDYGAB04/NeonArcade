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

        public async Task<Order> UpdateOrderAsync(int orderId, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if(existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found");
            }
            var validStatuses = new[] { "Pending", "Processing", "Completed", "Cancelled", "Refunded" };
            if (!validStatuses.Contains(order.Status))
                throw new ArgumentException($"Invalid status. Allowed: {string.Join(", ", validStatuses)}");
            
            if (existingOrder.Status == "Completed" || existingOrder.Status == "Cancelled")
                throw new InvalidOperationException($"Cannot update order with status '{existingOrder.Status}'");

            var oldStatus = existingOrder.Status;
            existingOrder.Status = order.Status;


            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Order {OrderNumber} status updated from {OldStatus} to {NewStatus}",
             existingOrder.OrderNumber, oldStatus, existingOrder.Status);

            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if(order == null)
            {
                return false;
            }
            if (order.Status == "Completed")
                throw new InvalidOperationException("Cannot delete completed orders");

            await _unitOfWork.Orders.DeleteAsync(orderId);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogWarning("Order {OrderNumber} deleted (ID: {OrderId})", 
             order.OrderNumber, orderId);

            return true;

        }

        public async Task<Order> CreateOrderFromCartAsync(string userId)
        {
            var cartItems = await _unitOfWork.Carts.GetCartWithGamesAsync(userId);
            if (!cartItems.Any())  // or cartItems.Count() == 0
            {
                throw new InvalidOperationException("Cannot create order from empty cart");
            }
            foreach (var cartItem in cartItems)
            {
                if (cartItem.Game == null)
                    throw new InvalidOperationException($"Game with ID {cartItem.GameId} not found");

                // Check 1: Is game available?
                if (!cartItem.Game.IsAvailable)
                    throw new InvalidOperationException($"Game '{cartItem.Game.Title}' is no longer available");

                // Check 2: Enough stock?
                if (cartItem.Game.StockQuantity < cartItem.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for '{cartItem.Game.Title}'");
            }
            var order = new Order
            {
                UserId = userId,
                OrderNumber = GenerateOrderNumber(),
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    GameId = cartItem.GameId,
                    Price = cartItem.Price,              
                    Quantity = cartItem.Quantity,
                    SubTotal = cartItem.SubTotal,
                    GameKey = GenerateGameKey(cartItem.GameId)  
                };
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.SubTotal);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Save order
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // 2. Clear cart
                await _unitOfWork.Carts.ClearCartAsync(userId);
                await _unitOfWork.SaveChangesAsync();

                // 3. Commit transaction
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create order from cart for user {UserId}", userId);
                throw;
            }

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