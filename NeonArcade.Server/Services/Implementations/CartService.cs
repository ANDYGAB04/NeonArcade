using NeonArcade.Server.Models;
using NeonArcade.Server.Models.DTOs;
using NeonArcade.Server.Models.Extensions;
using NeonArcade.Server.Repositories.Interfaces;
using NeonArcade.Server.Services.Interfaces;

namespace NeonArcade.Server.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;

        public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<CartItemResponse>> GetCartAsync(string userId)
        {
            var cartItems = await _unitOfWork.Carts.GetCartWithGamesAsync(userId);
            return cartItems.ToResponse();
        }

        public async Task<CartItemResponse> AddToCartAsync(string userId, int gameId, int quantity)
        {
            var game = await _unitOfWork.Games.GetByIdAsync(gameId);

            if (game == null)
                throw new KeyNotFoundException($"Game with ID {gameId} not found");

            if (!game.IsAvailable)
                throw new InvalidOperationException($"Game with ID {gameId} is not available for purchase");

            var existingItem = await _unitOfWork.Carts.GetCartItemAsync(userId, gameId);

            if (existingItem != null)
            {
                
                var newTotalQuantity = existingItem.Quantity + quantity;
                var hasStock = await _unitOfWork.Games.IsGameInStockAsync(gameId, newTotalQuantity);

                if (!hasStock)
                    throw new InvalidOperationException($"Insufficient stock for game with ID {gameId}. Requested total: {newTotalQuantity}, but only {game.StockQuantity} available.");

                existingItem.Quantity = newTotalQuantity;
                existingItem.SubTotal = existingItem.Price * existingItem.Quantity;
                existingItem.Game = game;
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("User {UserId} updated cart: Game {GameId} quantity from {OldQuantity} to {NewQuantity}", 
                    userId, gameId, existingItem.Quantity - quantity, existingItem.Quantity);
                
                return existingItem.ToResponse();
            }

            
            var hasStockForNew = await _unitOfWork.Games.IsGameInStockAsync(gameId, quantity);

            if (!hasStockForNew)
                throw new InvalidOperationException($"Insufficient stock for game with ID {gameId}");
            
            var cartItem = new CartItem
            {
                UserId = userId,
                GameId = gameId,
                Price = game.Price,
                Quantity = quantity,
                SubTotal = game.Price * quantity,
                Game = game 
            };
            
            await _unitOfWork.Carts.AddAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User {UserId} added {Quantity}x Game {GameId} to cart", userId, quantity, gameId);

            return cartItem.ToResponse();
        }

        public async Task<CartItemResponse> UpdateCartItemAsync(string userId, int gameId, int quantity)
        {
            var cartItem = await _unitOfWork.Carts.GetCartItemAsync(userId, gameId);

            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item for Game ID {gameId} not found in user {userId}'s cart");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var hasStock = await _unitOfWork.Games.IsGameInStockAsync(gameId, quantity);

            if (!hasStock)
                throw new InvalidOperationException($"Insufficient stock for game with ID {gameId}");

            cartItem.Quantity = quantity;
            cartItem.SubTotal = cartItem.Price * quantity;

            
            if (cartItem.Game == null)
            {
                cartItem.Game = await _unitOfWork.Games.GetByIdAsync(gameId);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User {UserId} updated Game {GameId} quantity to {Quantity} in cart", userId, gameId, quantity);

            return cartItem.ToResponse();
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int gameId)
        {
            var cartItem = await _unitOfWork.Carts.GetCartItemAsync(userId, gameId);

            if (cartItem == null)
                return false;

            await _unitOfWork.Carts.DeleteAsync(cartItem.Id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User {UserId} removed Game {GameId} from cart", userId, gameId);

            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            await _unitOfWork.Carts.ClearCartAsync(userId);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User {UserId} cleared their cart", userId);

            return true;
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            return await _unitOfWork.Carts.GetCartTotalAsync(userId);
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            return await _unitOfWork.Carts.GetCartItemCountAsync(userId);
        }

        public async Task<bool> IsGameInCartAsync(string userId, int gameId)
        {
            return await _unitOfWork.Carts.IsGameInCartAsync(userId, gameId);
        }
    }
}
