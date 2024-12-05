namespace hippserver.Models.Domain
{
    public class Driver
    {
        public int Id { get; set; }
        public string? Username { get; set; } 
        public string? Password { get; set; } 
        public string? LicensePlate { get; set; }
        public string? CarModel { get; set; }
        public Region Region { get; set; }
    }
}
