using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        public int FactoryId { get; set; }
        [ForeignKey(nameof(FactoryId))]
        public Factory? Factory { get; set; }

        public ICollection<OrderItem>? OrderItem { get; set; }
    }
}
