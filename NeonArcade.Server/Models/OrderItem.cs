namespace NeonArcade.Server.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GameId  { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }

        public string GameKey { get; set; } = string.Empty;

        public Order? Order { get; set; }

        public Game? Game { get; set; }
    }
}
