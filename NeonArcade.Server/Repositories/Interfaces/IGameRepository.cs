using NeonArcade.Server.Models;

namespace NeonArcade.Server.Repositories.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<IEnumerable<Game>> GetFeaturedGamesAsync(int count = 10);
        Task<IEnumerable<Game>> GetNewReleasesAsync(int count = 10);
        Task<IEnumerable<Game>> GetByGenreAsync(string genre);
        Task<IEnumerable<Game>> GetByPlatformAsync(string platform);
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);
        Task<IEnumerable<Game>> GetDiscountedGamesAsync();
        Task<Game?> GetGameWithDetailsAsync(int id);  
        Task<bool> IsGameInStockAsync(int gameId, int quantity);
    }
}
