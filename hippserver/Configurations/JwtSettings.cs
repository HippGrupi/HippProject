namespace hippserver.Configurations
{
    public class JwtSettings
    {
        public string Secret => Environment.GetEnvironmentVariable("JWT_SECRET");
        public string Issuer => Environment.GetEnvironmentVariable("JWT_ISSUER");
        public string Audience => Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        public int ExpirationInMinutes => int.Parse(Environment.GetEnvironmentVariable("JWT_TIMEOUT"));
    }
}