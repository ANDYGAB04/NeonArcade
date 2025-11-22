using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Game>> GetFeaturedGamesAsync(int count = 10)
        {
            return await _context.Games
                .Where(g => g.IsFeatured && g.IsAvailable)
                .OrderByDescending(g => g.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetNewReleasesAsync(int count = 10)
        {
            return await _context.Games
                .Where(g => g.IsAvailable && g.ReleaseDate <= DateTime.UtcNow)
                .OrderByDescending(g => g.ReleaseDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByGenreAsync(string genre)
        {
            return await _context.Games
                .Where(g => g.IsAvailable && g.Genres.Contains(genre))
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByPlatformAsync(string platform)
        {
            return await _context.Games
                .Where(g => g.IsAvailable && g.Platforms.Contains(platform))
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<Game>();
            }

            var lowerSearchTerm = searchTerm.ToLower();

            return await _context.Games
                .Where(g => g.IsAvailable &&
                    (g.Title.ToLower().Contains(lowerSearchTerm) ||
                     g.Description.ToLower().Contains(lowerSearchTerm) ||
                     g.Developer.ToLower().Contains(lowerSearchTerm)))
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetDiscountedGamesAsync()
        {
            return await _context.Games
                .Where(g => g.IsAvailable && 
                       g.DiscountPrice.HasValue && 
                       g.DiscountPrice < g.Price)
                .OrderByDescending(g => (g.Price - g.DiscountPrice!.Value) / g.Price)
                .ToListAsync();
        }

        public async Task<Game?> GetGameWithDetailsAsync(int id)
        {
            return await _context.Games
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsGameInStockAsync(int gameId, int quantity)
        {
            return await _context.Games
                .AnyAsync(g => g.Id == gameId && 
                          g.IsAvailable && 
                          g.StockQuantity >= quantity);
        }
    }
}
