using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.Admin
{
    public class ChangePasswordDTO
    {
        [Required, EmailAddress]
        public  string Email { get; set; } = string.Empty;
        [Required]
        public  string CurrentPassword { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public  string NewPassword { get; set; } = string.Empty;
    }
}
