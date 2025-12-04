using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeonArcade.Server.Models.DTOs;
using NeonArcade.Server.Models.Extensions;
using NeonArcade.Server.Services.Interfaces;
using System.Security.Claims;

namespace NeonArcade.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Get all orders for the authenticated user
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            var ordersResponse = orders.ToResponse();  // ✅ Convert to DTOs
            
            return Ok(ordersResponse);
        }

        /// <summary>
        /// Get order details by ID (user can only view their own orders)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponse>> GetOrderById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var order = await _orderService.GetOrderWithDetailsAsync(id);
            
            if (order == null)
                return NotFound(new { message = $"Order with ID {id} not found" });

            // Security: Users can only view their own orders
            if (order.UserId != userId)
                return Forbid();

            var orderResponse = order.ToResponse();
            return Ok(orderResponse);
        }

        /// <summary>
        /// Create order from user's cart (Checkout)
        /// </summary>
        [HttpPost("checkout")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderResponse>> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                var order = await _orderService.CreateOrderFromCartAsync(userId);
                var orderResponse = order.ToResponse();
                
                return CreatedAtAction(
                    nameof(GetOrderById), 
                    new { id = order.Id }, 
                    orderResponse);
            }
            catch (InvalidOperationException ex)
            {
                // Handle business rule violations (empty cart, out of stock, etc.)
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update order status (Admin only)
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponse>> UpdateOrderStatus(
            int id, 
            [FromBody] UpdateOrderStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Create a minimal Order object with just the status
                var orderUpdate = new Models.Order { Status = request.Status };
                
                var updatedOrder = await _orderService.UpdateOrderAsync(id, orderUpdate);
                var orderResponse = updatedOrder.ToResponse();
                
                return Ok(orderResponse);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete order (Admin only, cannot delete completed orders)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                
                if (!result)
                    return NotFound(new { message = $"Order with ID {id} not found" });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Handle business rule violations (can't delete completed orders)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
