using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Salary
    {
        [Key]
        public int SalaryId { get; set; }

        public decimal BaseSalary { get; set; }

        public decimal Bonus { get; set; }

        public decimal Deductions { get; set; }

        public string? Month {  get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}
