using hippserver.Models.JunctionTables;

namespace hippserver.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? StoredAt { get; set; }
        public ICollection<ProductWarehouse> ProductWarehouse { get; set; } = new List<ProductWarehouse>();

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
