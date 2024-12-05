using hippserver.Models.JunctionTables;

namespace hippserver.Models.Domain
{
    public class SalesPerson
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        
        public ICollection<Order>? Orders { get; set; }
        public Region Region { get; set; }
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
