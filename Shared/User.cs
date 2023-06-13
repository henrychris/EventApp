using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class User
    {
        public static User NotFound = new() { };

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
        public decimal WalletBalance { get; set; } = 0;
        public string Role { get; set; } = UserRoles.User.ToString();
    }
}
