using NeonArcade.Server.Data;
using NeonArcade.Server.Models;
using NeonArcade.Server.Repositories.Interfaces;

namespace NeonArcade.Server.Repositories.Implementations
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Game>> GetByGenreAsync(string genre)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> GetByPlatformAsync(string platform)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> GetDiscountedGamesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> GetFeaturedGamesAsync(int count = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<Game?> GetGameWithDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> GetNewReleasesAsync(int count = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsGameInStockAsync(int gameId, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
