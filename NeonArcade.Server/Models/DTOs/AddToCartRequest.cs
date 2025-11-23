using System.ComponentModel.DataAnnotations;

namespace NeonArcade.Server.Models.DTOs
{
    public class AddToCartRequest
    {
        [Required(ErrorMessage = "Game ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Game ID must be greater than 0")]
        public int GameId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
