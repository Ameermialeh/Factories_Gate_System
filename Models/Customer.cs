using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Customer 
    {
        [Key]
        public int CustomerId { get; set; }
        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, StringLength(300)]
        public required string Address { get; set; }

        [Required]
        public required string Phone { get; set; }
         
        public ICollection<Order>? Orders { get; set; }
    }
}
