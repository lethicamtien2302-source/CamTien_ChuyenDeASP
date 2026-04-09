namespace ConnectDB.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string Method { get; set; } // COD, MOMO

        public string Status { get; set; } // Pending, Paid
    }
}
