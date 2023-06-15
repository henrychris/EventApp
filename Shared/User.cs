using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public decimal WalletBalance { get; set; } = 0.00m;
        public string Role { get; set; } = UserRoles.User.ToString();
    }

    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal WalletBalance { get; set; } = 0;
        public string Role { get; set; } = UserRoles.User.ToString();
    }

    public class UpdateUserRequest
    {
        [MaxLength(50)]
        public string? FirstName { get; set; } = null;
        [MaxLength(50)]
        public string? LastName { get; set; } = null;
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; } = null;
    }
}
