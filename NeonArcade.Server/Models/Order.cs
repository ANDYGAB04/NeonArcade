namespace NeonArcade.Server.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public User? User { get; set; }

        public List<OrderItem>? OrderItems { get; set; }





    }
}
