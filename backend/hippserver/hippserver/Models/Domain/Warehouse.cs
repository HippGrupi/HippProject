namespace hippserver.Models.Domain
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Region Region { get; set; }
        
    }
}
