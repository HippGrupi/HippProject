namespace hippserver.Models.Domain
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public OrderStatus OldStatus { get; set; }
        public OrderStatus NewStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
