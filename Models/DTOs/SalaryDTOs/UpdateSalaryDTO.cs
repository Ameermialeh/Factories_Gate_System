namespace FactoriesGateSystem.Models.DTOs.SalaryDTOs
{
    public class UpdateSalaryDTO
    {
        public decimal? BaseSalary { get; set; }

        public decimal? Bonus { get; set; }

        public decimal? Deductions { get; set; }

        public DateTime? Date { get; set; }
    
    }
}
