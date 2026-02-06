using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.ExpenseDTOs
{
    public class AddExpenseDTO
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
