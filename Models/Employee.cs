using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Employee  
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Phone { get; set; }

        public ICollection<Vacation>? Vacations { get; set; }
    }
}
