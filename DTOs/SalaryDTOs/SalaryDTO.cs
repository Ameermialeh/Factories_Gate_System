namespace FactoriesGateSystem.DTOs.SalaryDTOs
{
    public class SalaryDTO
    {
        public int Id { get; set; }

        public decimal BaseSalary { get; set; }

        public decimal Bonus { get; set; }

        public decimal Deductions { get; set; }

        public string? Month { get; set; }

        public int EmployeeId { get; set; }
    }
}
