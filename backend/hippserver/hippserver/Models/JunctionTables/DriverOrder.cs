using hippserver.Models.Domain;
using hippserver.Models.Enums;

namespace hippserver.Models.JunctionTables
{
    public class DriverOrder
    {
        
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public DateTime AssignedAt { get; set; }
        public OrderStatus Status { get; set; } 
    }
}
