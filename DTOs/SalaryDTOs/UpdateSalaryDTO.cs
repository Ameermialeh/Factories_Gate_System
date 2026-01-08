namespace FactoriesGateSystem.DTOs.SalaryDTOs
{
    public class UpdateSalaryDTO
    {
        public class UpdateSalariesDTO
        {
            public int Id { get; set; }
            public decimal BaseSalary { get; set; }

            public decimal Bonus { get; set; }

            public decimal Deductions { get; set; }
        }

        public class UpdateDateSalaryDTO
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
