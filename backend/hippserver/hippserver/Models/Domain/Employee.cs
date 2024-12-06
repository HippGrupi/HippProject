using hippserver.Models.Enums;

namespace hippserver.Models.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Manager? Supervisor { get; set; }
        
        public string? Department { get; set; }
        public Region Region { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Product? Product { get; set; }
    }
}
