namespace Shared
{
    public enum UserRoles
    {
        User,
        Admin
    }

    public static class UserRoleStrings
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly string[] AllRoles = { Admin, User };
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
