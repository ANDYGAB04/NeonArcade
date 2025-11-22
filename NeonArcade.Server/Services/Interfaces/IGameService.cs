using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;

namespace NeonArcade.Server.Services.Interfaces
{
    public interface IGameService
    {
        Task<PagedResult<Game>> GetGamesAsync(GameQueryParameters parameters);
        Task<Game?> GetGameByIdAsync(int id);
        Task<IEnumerable<Game>> GetFeaturedGamesAsync(int count = 10);
        Task<IEnumerable<Game>> GetNewReleasesAsync(int count = 10);
        Task<IEnumerable<Game>> GetDiscountedGamesAsync();
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);
        Task<IEnumerable<Game>> GetGamesByGenreAsync(string genre);
        Task<IEnumerable<Game>> GetGamesByPlatformAsync(string platform);
        Task<Game> CreateGameAsync(Game game);
        Task<Game> UpdateGameAsync(int id, Game game);
        Task<bool> DeleteGameAsync(int id);
        Task<bool> IsGameInStockAsync(int gameId, int quantity);
    }
}
