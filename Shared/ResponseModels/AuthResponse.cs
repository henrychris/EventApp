namespace Shared.ResponseModels
{
    public class AuthResponse
    {
        public static AuthResponse NotFound = new();
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal WalletBalance { get; set; } = 0;
        public string Role { get; set; } = UserRoles.User.ToString();
        public string? Token { get; set; } = null;
    }
}
