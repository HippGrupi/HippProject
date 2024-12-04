namespace hippserver.Helpers
{
    public static class EnvHelper
    {
        public static string GetConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(database)
                || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Database configuration missing in environment variables");
            }

            return $"Server={server};Database={database};User Id={user};Password={password};TrustServerCertificate=True;";
        }
    }
}