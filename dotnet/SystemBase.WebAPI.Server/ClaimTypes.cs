namespace SystemBase;

public static class ClaimTypes
{
    public static class User
    {
        public static string Id { get; set; }           = "user.id";
        public static string Username { get; set; }     = "user.username";
        public static string FullName { get; set; }     = "user.fullname";
    }
}