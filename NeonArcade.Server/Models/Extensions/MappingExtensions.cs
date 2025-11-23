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
}
