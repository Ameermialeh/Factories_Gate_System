using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs
{
    public class AuthDTO
    {
        public class RegisterDTO
        {
            [Required]
            public string Name { get; set; } = string.Empty;
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required, MinLength(6)]
            public string Password { get; set; } = string.Empty;
            [Required]
            public string FactoryName { get; set; } = string.Empty;
            [Required]
            public string Address { get; set; } = string.Empty;
        }

        public class LoginDTO
        {
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required, EmailAddress]
            public string Password { get; set; } = string.Empty;
        }

        public class RefreshTokenDTO
        {
            [Required]
            public string RefreshToken { get; set; } = string.Empty;
        }
    }
}
