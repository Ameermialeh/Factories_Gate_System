using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        [Required]
        public required string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date {  get; set; }

        public int FactoryId { get; set; }

        [ForeignKey(nameof(FactoryId))]
        public Factory? Factory { get; set; }
    }
}
