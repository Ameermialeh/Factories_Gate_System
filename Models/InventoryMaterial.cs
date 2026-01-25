using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class InventoryMaterial
    {
        [Key]
        public int InventoryId { get; set; }

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<Material>? Materials { get; set; }
    }
}
