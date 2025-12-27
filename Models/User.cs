using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public required string Email { get; set; }

        [StringLength(200)]
        public required string PasswordHash { get; set; }

        public string Role { get; set; } = "manager";

        public List<RefreshToken> RefreshTokens { get; set; }

    }
}
