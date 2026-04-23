namespace ConnectDB.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public int? UserId { get; set; }
        public User? User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
