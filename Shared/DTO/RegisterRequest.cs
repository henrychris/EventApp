using System.ComponentModel.DataAnnotations;

namespace ErrandPay_Test.Shared.DTOs
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
