namespace NeonArcade.Server.Models
{
    public class Game
    {
        //Basic Info
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        //Price
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }
        

        //Media Assets
        public string CoverImageUrl { get; set; } = string.Empty;

        public string? TrailerUrl { get; set; } = string.Empty;
        public List<string> ScreenshotUrls { get; set; } = new List<string>();

        //Metadata

        public string Developer { get; set; } = string.Empty;

        public string Publisher { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        public string AgeRating { get; set; } = string.Empty;

        public List<string> Genres { get; set; } = new List<string>();

        public List<string> Platforms { get; set; } = new List<string>();

        public List<string> Tags { get; set; } = new List<string>();

        //System Requirements
        public string MinimumRequirements { get; set; } = string.Empty;
        public string RecommendedRequirements { get; set; } = string.Empty;

        //Inventory
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }

        public bool IsFeatured { get; set; }

        //Timestamps:
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

}
