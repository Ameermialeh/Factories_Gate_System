using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public required string Name { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public int Price { get; set; }

        public int FactoryId { get; set; }

        [ForeignKey(nameof(FactoryId))]
        public Factory? Factory { get; set; }

        public ICollection<OrderItem>? OrderProducts { get; set; }
    }
}
