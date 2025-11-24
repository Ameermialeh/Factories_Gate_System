using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class User
    {
        [Key]
        int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreateAt { get; set; }
    }
}
