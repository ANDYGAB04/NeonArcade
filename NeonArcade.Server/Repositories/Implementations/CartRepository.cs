using Microsoft.EntityFrameworkCore;
using NeonArcade.Server.Data;
using NeonArcade.Server.Models;
using NeonArcade.Server.Repositories.Interfaces;

namespace NeonArcade.Server.Repositories.Implementations
{
    public class CartRepository : Repository<CartItem>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartByUserIdAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemAsync(string userId, int gameId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.GameId == gameId);
        }

        public async Task<bool> IsGameInCartAsync(string userId, int gameId)
        {
            return await _context.CartItems
                .AnyAsync(ci => ci.UserId == userId && ci.GameId == gameId);
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.SubTotal);
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<IEnumerable<CartItem>> GetCartWithGamesAsync(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Game)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            return cartItem;
        }
    }
}
