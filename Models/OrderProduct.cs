using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class OrderProduct
    {
        [Required]
        public int OrderId  { get; set; }
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order order { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product product { get; set; }
        
        public int Quantity { get; set; }
    }
}
