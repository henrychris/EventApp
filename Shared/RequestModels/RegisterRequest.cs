﻿using System.ComponentModel.DataAnnotations;

namespace Shared.RequestModels
{
    public class RegisterRequest
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = UserRoles.User.ToString();
    }
}
