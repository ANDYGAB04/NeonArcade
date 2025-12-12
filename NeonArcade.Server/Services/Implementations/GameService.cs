using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;
using NeonArcade.Server.Repositories.Interfaces;
using NeonArcade.Server.Services.Interfaces;

namespace NeonArcade.Server.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GameService> _logger;

        public GameService(IUnitOfWork unitOfWork, ILogger<GameService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PagedResult<Game>> GetGamesAsync(GameQueryParameters parameters)
        {
            return await _unitOfWork.Games.GetGamesFilteredAsync(parameters);
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _unitOfWork.Games.GetGameWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Game>> GetFeaturedGamesAsync(int count = 10)
        {
            return await _unitOfWork.Games.GetFeaturedGamesAsync(count);
        }

        public async Task<IEnumerable<Game>> GetNewReleasesAsync(int count = 10)
        {
            return await _unitOfWork.Games.GetNewReleasesAsync(count);
        }

        public async Task<IEnumerable<Game>> GetDiscountedGamesAsync()
        {
            return await _unitOfWork.Games.GetDiscountedGamesAsync();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be empty", nameof(searchTerm));
            }

            return await _unitOfWork.Games.SearchGamesAsync(searchTerm);
        }

        public async Task<IEnumerable<Game>> GetGamesByGenreAsync(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                throw new ArgumentException("Genre cannot be empty", nameof(genre));
            }

            return await _unitOfWork.Games.GetByGenreAsync(genre);
        }

        public async Task<IEnumerable<Game>> GetGamesByPlatformAsync(string platform)
        {
            if (string.IsNullOrWhiteSpace(platform))
            {
                throw new ArgumentException("Platform cannot be empty", nameof(platform));
            }

            return await _unitOfWork.Games.GetByPlatformAsync(platform);
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            if (game.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(game.Price));
            }

            if (game.DiscountPrice.HasValue && game.DiscountPrice >= game.Price)
            {
                throw new ArgumentException("Discount price must be less than regular price");
            }

            if (game.StockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative");
            }

            game.CreatedAt = DateTimeOffset.UtcNow;
            game.UpdatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.Games.GetAllAsync();
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Game created: {GameTitle} (ID: {GameId})", game.Title, game.Id);

            return game;
        }

        public async Task<Game> UpdateGameAsync(int id, Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var existingGame = await _unitOfWork.Games.GetByIdAsync(id);
            if (existingGame == null)
            {
                throw new KeyNotFoundException($"Game with ID {id} not found");
            }

            if (game.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            if (game.DiscountPrice.HasValue && game.DiscountPrice >= game.Price)
            {
                throw new ArgumentException("Discount price must be less than regular price");
            }

            if (game.StockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative");
            }

            existingGame.Title = game.Title;
            existingGame.Description = game.Description;
            existingGame.ShortDescription = game.ShortDescription;
            existingGame.Price = game.Price;
            existingGame.DiscountPrice = game.DiscountPrice;
            existingGame.CoverImageUrl = game.CoverImageUrl;
            existingGame.TrailerUrl = game.TrailerUrl;
            existingGame.ScreenshotUrls = game.ScreenshotUrls;
            existingGame.Developer = game.Developer;
            existingGame.Publisher = game.Publisher;
            existingGame.ReleaseDate = game.ReleaseDate;
            existingGame.AgeRating = game.AgeRating;
            existingGame.Genres = game.Genres;
            existingGame.Platforms = game.Platforms;
            existingGame.Tags = game.Tags;
            existingGame.MinimumRequirements = game.MinimumRequirements;
            existingGame.RecommendedRequirements = game.RecommendedRequirements;
            existingGame.StockQuantity = game.StockQuantity;
            existingGame.IsAvailable = game.IsAvailable;
            existingGame.IsFeatured = game.IsFeatured;
            existingGame.UpdatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Game updated: {GameTitle} (ID: {GameId})", existingGame.Title, existingGame.Id);

            return existingGame;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _unitOfWork.Games.GetByIdAsync(id);
            if (game == null)
            {
                return false;
            }

            await _unitOfWork.Games.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Game deleted: {GameTitle} (ID: {GameId})", game.Title, id);

            return true;
        }

        public async Task<bool> IsGameInStockAsync(int gameId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            return await _unitOfWork.Games.IsGameInStockAsync(gameId, quantity);
        }
    }
}
