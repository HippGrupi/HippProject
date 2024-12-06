using hippserver.Models.Enums;
using hippserver.Models.JunctionTables;

namespace hippserver.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? DeliveryDestination { get; set; }
        public DateTime LastUpdated { get; set; }
        public Client? Client { get; set; }
        public SalesPerson? SalesPerson { get; set; }
        public Employee? Employee { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderProduct> OrderProducts{ get; set; } = new List<OrderProduct>();
        public ICollection<DriverOrder> DriverOrders { get; set; } = new List<DriverOrder>();
    }
}
