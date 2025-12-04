namespace NeonArcade.Server.Models.DTOs
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; } = string.Empty;
        public string GameKey { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }        public GameSummary? Game { get; set; }


    }
}