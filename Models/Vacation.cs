using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Vacation
    {
        [Key]
        public int VacationId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime VacationDate { get; set; }
        [Required]
        public required string VacationReason { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}
