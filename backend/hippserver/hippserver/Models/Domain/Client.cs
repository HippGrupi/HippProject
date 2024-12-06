using hippserver.Models.Enums;
using hippserver.Models.JunctionTables;

namespace hippserver.Models.Domain
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public int SalesPersonId { get; set; } 
        public SalesPerson? SalesPerson { get; set; } 
    }
}
