using Microsoft.EntityFrameworkCore;
using NeonArcade.Server.Data;
using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;
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

        public async Task<PagedResult<Game>> GetGamesFilteredAsync(GameQueryParameters parameters)
        {
            IQueryable<Game> query = _context.Games;

            // Search filter
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(g => 
                    g.Title.ToLower().Contains(searchTerm) ||
                    g.Description.ToLower().Contains(searchTerm));
            }

            // Price filters
            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(g => g.Price >= parameters.MinPrice.Value);
            }
            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(g => g.Price <= parameters.MaxPrice.Value);
            }

            // Availability filter
            if (parameters.IsAvailable.HasValue)
            {
                query = query.Where(g => g.IsAvailable == parameters.IsAvailable.Value);
            }

            // Featured filter
            if (parameters.IsFeatured.HasValue)
            {
                query = query.Where(g => g.IsFeatured == parameters.IsFeatured.Value);
            }

            // For genre filter, we need to handle it differently since Genres is stored as JSON
            // First, get the filtered list, then apply genre filter in memory
            List<Game> allFilteredGames;
            
            if (!string.IsNullOrEmpty(parameters.Genre))
            {
                // Fetch all games matching other filters, then filter by genre in memory
                allFilteredGames = await query.ToListAsync();
                allFilteredGames = allFilteredGames
                    .Where(g => g.Genres.Any(genre => 
                        genre.Equals(parameters.Genre, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
            else
            {
                allFilteredGames = await query.ToListAsync();
            }

            // Get total count after genre filter
            var totalCount = allFilteredGames.Count;

            // Apply sorting in memory
            IEnumerable<Game> sortedGames = parameters.SortBy?.ToLower() switch
            {
                "title" => allFilteredGames.OrderBy(g => g.Title),
                "titledesc" => allFilteredGames.OrderByDescending(g => g.Title),
                "price" => allFilteredGames.OrderBy(g => g.DiscountPrice ?? g.Price),
                "pricedesc" => allFilteredGames.OrderByDescending(g => g.DiscountPrice ?? g.Price),
                "releasedate" => allFilteredGames.OrderByDescending(g => g.ReleaseDate),
                _ => allFilteredGames.OrderByDescending(g => g.CreatedAt)
            };

            // Apply pagination in memory
            var items = sortedGames
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PagedResult<Game>(items, totalCount, parameters.PageNumber, parameters.PageSize);
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
