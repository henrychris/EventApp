using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = Roles.User.ToString();
    }
}
