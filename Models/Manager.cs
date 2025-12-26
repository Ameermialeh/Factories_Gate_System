using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FactoriesGateSystem.Models
{
    public class Manager 
    {
        [Key]
        public int ManagerId { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public required string Email { get; set; }

        [StringLength(200)]
        public required string PasswordHash { get; set; }

        public string? Role { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
