using NeonArcade.Server.Models.DTOs;

namespace NeonArcade.Server.Models.Extensions
{
    public static class CartItemExtensions
    {
        public static CartItemResponse ToResponse(this CartItem cartItem)
        {
            if (cartItem == null)
                throw new ArgumentNullException(nameof(cartItem));

            return new CartItemResponse
            {
                Id = cartItem.Id,
                GameId = cartItem.GameId,
                Price = cartItem.Price,
                Quantity = cartItem.Quantity,
                SubTotal = cartItem.SubTotal,
                Game = cartItem.Game != null ? cartItem.Game.ToSummary() : new GameSummary { Id = cartItem.GameId }
            };
        }

        public static IEnumerable<CartItemResponse> ToResponse(this IEnumerable<CartItem> cartItems)
        {
            return cartItems.Select(ci => ci.ToResponse());
        }
    }

    public static class GameExtensions
    {
        public static GameSummary ToSummary(this Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            return new GameSummary
            {
                Id = game.Id,
                Title = game.Title,
                ShortDescription = game.ShortDescription,
                CoverImageUrl = game.CoverImageUrl,
                Price = game.Price,
                DiscountPrice = game.DiscountPrice,
                IsAvailable = game.IsAvailable,
                StockQuantity = game.StockQuantity,
                Platforms = game.Platforms ?? new List<string>(),
                Genres = game.Genres ?? new List<string>()
            };
        }
    }

    public static class OrderExtensions
    {
        public static OrderResponse ToResponse(this Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            return new OrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status= order.Status,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId= order.UserId,
                UserEmail = order.User?.Email?? string.Empty,
                UserFullName = order.User != null
                  ? $"{order.User.FirstName} {order.User.LastName}".Trim()
                  : string.Empty,
                OrderItems = order.OrderItems?.Select(oi => oi.ToResponse()).ToList() ?? new()

            };

        }
        public static IEnumerable<OrderResponse> ToResponse(this IEnumerable<Order> orders)
        {
            return orders.Select(o => o.ToResponse());
        }

    }
    public static class OrderItemExtensions
    {
        public static OrderItemResponse ToResponse(this OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));
            return new OrderItemResponse
            {
                Id = orderItem.Id,
                GameId = orderItem.GameId,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity,
                SubTotal = orderItem.SubTotal,
                GameKey = orderItem.GameKey,
                GameTitle = orderItem.Game?.Title ?? string.Empty,
                Game = orderItem.Game?.ToSummary()
            };
        }
      
    }
}
