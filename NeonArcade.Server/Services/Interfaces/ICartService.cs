using NeonArcade.Server.Models;

namespace NeonArcade.Server.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartAsync (string userId);
        Task<CartItem> AddToCartAsync (string userId, int gameId, int quantity);
        Task<CartItem> UpdateCartItemAsync (string userId, int gameId, int quantity);
        Task<bool> RemoveFromCartAsync (string userId, int gameId);
        Task<bool> ClearCartAsync (string userId);
        Task <decimal> GetCartTotalAsync (string userId);
        Task <int> GetCartItemCountAsync (string userId);
        Task <bool> IsGameInCartAsync (string userId, int gameId);

    }
}
