using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }

        public string Role {  get; set; } = "admin";
    }
}
