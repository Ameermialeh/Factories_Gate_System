namespace FactoriesGateSystem.DTOs.ExpenseDTOs
{
    public class AddExpenseDTO
    {
        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
