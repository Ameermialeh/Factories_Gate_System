using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public int InventoryId { get; set; }
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string NameAr { get; set; }

        [Required]
        public int Price { get; set; }

        public int FactoryId { get; set; }

        [ForeignKey(nameof(FactoryId))]
        public Factory? Factory { get; set; }

        public ICollection<OrderItem>? OrderProducts { get; set; }

        [ForeignKey(nameof(InventoryId))]
        public InventoryProduct? Inventory { get; set; }
    }
}
