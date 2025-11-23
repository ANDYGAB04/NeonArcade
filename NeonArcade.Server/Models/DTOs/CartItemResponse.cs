using System.ComponentModel.DataAnnotations;

namespace NeonArcade.Server.Models.DTOs
{
    public class CartItemResponse
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public decimal Price { get; set; }
        
        [Range(1, 100)]
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        
        // Game details without circular reference
        public GameSummary Game { get; set; } = null!;
    }

    public class GameSummary
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int StockQuantity { get; set; }
        public List<string> Platforms { get; set; } = new();
        public List<string> Genres { get; set; } = new();
    }
}
