namespace FactoriesGateSystem.Models.DTOs.SalaryDTOs
{
    public class AddSalaryDTO
    {
        public int EmployeeId { get; set; }

        public decimal BaseSalary { get; set; }

        public decimal Bonus { get; set; }

        public decimal Deductions { get; set; }

        public DateTime Date { get; set; }
    }
}
