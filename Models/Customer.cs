using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Customer 
    {
        [Key]
        public int CustomerId { get; set; }
        [Required, StringLength(100)]
        public required string CustomerName { get; set; }

        [Required, StringLength(300)]
        public required string Address { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        public int CurrentBalance { get; set; } = 0;

        public UserType access() => UserType.Customer;
         

        public ICollection<Order>? Orders { get; set; }
    }
}
