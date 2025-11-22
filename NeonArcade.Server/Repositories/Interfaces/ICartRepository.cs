using NeonArcade.Server.Models;

namespace NeonArcade.Server.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartByUserIdAsync(string userId);
        Task<CartItem?> GetCartItemAsync(string userId, int gameId);
        Task<bool> IsGameInCartAsync(string userId, int gameId);
        Task ClearCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<IEnumerable<CartItem>> GetCartWithGamesAsync(string userId);  // Include Game details
    }
}
