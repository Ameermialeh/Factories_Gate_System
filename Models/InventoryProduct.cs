using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class InventoryProduct
    {
        [Key]
        public int InventoryId { get; set; }

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<Product>? Product { get; set; }

    }
}
