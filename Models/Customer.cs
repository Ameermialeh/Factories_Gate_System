using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Customer : User
    {
        [Required]
        public string PhoneNumber { get; set; }

        public int CurrentBalance { get; set; } = 0;

        public override UserType access() => UserType.Customer;
         

        public ICollection<Order> Orders { get; set; }
    }
}
