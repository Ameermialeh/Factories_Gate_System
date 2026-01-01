using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        public int FactoryId { get; set; }
        [ForeignKey(nameof(FactoryId))]

        public ICollection<Vacation>? Vacations { get; set; }
    }
}
