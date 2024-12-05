namespace hippserver.Models.Domain
{
    public class SalesPerson
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Region { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
