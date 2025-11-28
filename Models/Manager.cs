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
        public string ManagerName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string ManagerEmail { get; set; }

        [StringLength(200)]
        public string ManagerPassword { get; set; }

        public bool IsActive { get; set; } = true;

        public UserType access() => UserType.Manager;  
 
    }
}
