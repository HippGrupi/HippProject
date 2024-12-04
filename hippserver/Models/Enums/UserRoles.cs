namespace hippserver.Models.Enums
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Menaxher = "Menaxher";
        public const string Komercialist = "Komercialist";
        public const string Shofer = "Shofer";
        public const string Etiketues = "Etiketues";

        public static readonly IReadOnlyList<string> AllRoles = new[]
        {
            Admin,
            Menaxher,
            Komercialist,
            Shofer,
            Etiketues
        };

        public static bool IsValidRole(string role)
        {
            return AllRoles.Contains(role);
        }
    }
}