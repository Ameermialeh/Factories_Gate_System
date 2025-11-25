using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string OrderName { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public string OrderDescription { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
