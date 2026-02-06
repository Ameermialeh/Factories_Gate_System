using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.SalaryDTOs
{
    public class AddSalaryDTO
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public decimal BaseSalary { get; set; }
        [Required]
        public decimal Bonus { get; set; }
        [Required]
        public decimal Deductions { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
