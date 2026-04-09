namespace ConnectDB.Models
{
    public class Address
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string AddressLine { get; set; }

        public string Phone { get; set; }
    }
}
