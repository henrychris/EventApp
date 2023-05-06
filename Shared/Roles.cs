namespace Shared
{
    public enum RoleEnum
    {
        User,
        Admin
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
