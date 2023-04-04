using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
