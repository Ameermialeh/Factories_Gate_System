using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }

        public int MaterialId { get; set; }

        public int ProductId { get; set; }


        [ForeignKey(nameof(MaterialId))]
        public Material? Material { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
