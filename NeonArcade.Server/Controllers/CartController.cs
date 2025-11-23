using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeonArcade.Server.Models.DTOs;
using NeonArcade.Server.Services.Interfaces;
using System.Security.Claims;

namespace NeonArcade.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Get all items in the authenticated user's cart
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CartItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CartItemResponse>>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var cartItems = await _cartService.GetCartAsync(userId);
            return Ok(cartItems);
        }

        /// <summary>
        /// Get the total price of all items in cart
        /// </summary>
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<object>> GetCartTotal()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var total = await _cartService.GetCartTotalAsync(userId);
            return Ok(new { total });
        }

        /// <summary>
        /// Get the total quantity of items in cart
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<object>> GetCartItemCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var itemCount = await _cartService.GetCartItemCountAsync(userId);
            return Ok(new { count = itemCount });
        }

        /// <summary>
        /// Check if a specific game is in the user's cart
        /// </summary>
        [HttpGet("check/{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<object>> IsGameInCart(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var isInCart = await _cartService.IsGameInCartAsync(userId, gameId);
            return Ok(new { gameId, isInCart });
        }

        /// <summary>
        /// Add a game to cart or increase quantity if already exists
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItemResponse>> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItem = await _cartService.AddToCartAsync(userId, request.GameId, request.Quantity);
            return Ok(cartItem);
        }

        /// <summary>
        /// Update the quantity of a cart item
        /// </summary>
        [HttpPut("{gameId}")]
        [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItemResponse>> UpdateCartItem(int gameId, [FromBody] UpdateCartItemRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItem = await _cartService.UpdateCartItemAsync(userId, gameId, request.Quantity);
            return Ok(cartItem);
        }

        /// <summary>
        /// Remove a specific game from cart
        /// </summary>
        [HttpDelete("{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> RemoveFromCart(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var result = await _cartService.RemoveFromCartAsync(userId, gameId);
            
            if (!result)
                return NotFound(new { message = $"Game with ID {gameId} not found in cart" });
            
            return Ok(new { message = "Item removed from cart", gameId });
        }

        /// <summary>
        /// Clear all items from cart
        /// </summary>
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<object>> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var result = await _cartService.ClearCartAsync(userId);
            return Ok(new { message = "Cart cleared successfully" });
        }
    }
}
