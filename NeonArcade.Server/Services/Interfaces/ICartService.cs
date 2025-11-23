using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;

namespace NeonArcade.Server.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemResponse>> GetCartAsync(string userId);
        Task<CartItemResponse> AddToCartAsync(string userId, int gameId, int quantity);
        Task<CartItemResponse> UpdateCartItemAsync(string userId, int gameId, int quantity);
        Task<bool> RemoveFromCartAsync(string userId, int gameId);
        Task<bool> ClearCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<bool> IsGameInCartAsync(string userId, int gameId);
    }
}
