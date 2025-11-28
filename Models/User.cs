/*
 using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Password { get; set; }
        [Required, StringLength(300)]
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreateAt { get; set; }
    }
}
*/