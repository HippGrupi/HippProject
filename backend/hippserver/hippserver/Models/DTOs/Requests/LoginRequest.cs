namespace hippserver.Models.DTOs.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty; // Non-nullable
        public string Password { get; set; } = string.Empty; // Non-nullable
        
    }
}