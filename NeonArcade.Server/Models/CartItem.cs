namespace NeonArcade.Server.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        public User? User { get; set; }

        public Game? Game { get; set; }
    }
}
