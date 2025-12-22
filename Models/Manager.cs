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
        public required string ManagerName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public required string ManagerEmail { get; set; }

        [StringLength(200)]
        public required string ManagerPassword { get; set; }

        public UserType access() => UserType.Manager;  
 
    }
}
