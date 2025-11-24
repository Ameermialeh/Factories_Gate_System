using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public int CurrentBalance { get; set; } = 0;
        public DateTime? CreateAt { get; set; }
    }
}
