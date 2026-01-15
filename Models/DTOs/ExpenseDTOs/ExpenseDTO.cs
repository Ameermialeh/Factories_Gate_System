using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.ExpenseDTOs
{
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
