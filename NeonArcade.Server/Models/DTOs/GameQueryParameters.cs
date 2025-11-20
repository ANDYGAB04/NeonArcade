namespace NeonArcade.Server.Models.DTOs
{
    public class GameQueryParameters
    {
        private const int MaxPageSize = 50;
        public string? SearchTerm { get; set; }
        public string? Genre { get; set; }
        public string? Platform { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsFeatured { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "desc";

        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;  // Ensure >= 1
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;  // Cap at 50
        }
      
    }
}
