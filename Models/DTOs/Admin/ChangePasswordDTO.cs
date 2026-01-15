using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.Admin
{
    public class ChangePasswordDTO
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string CurrentPassword { get; set; }
        [Required, MinLength(8)]
        public required string NewPassword { get; set; }
    }
}
