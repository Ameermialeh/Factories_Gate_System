using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class SupplierMaterial
    {
        [Key]
        public int SupplierMaterialId { get; set; }

        [Required]
        public int SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; }

        [Required]
        public int MaterialId { get; set; }
        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; }

        [Range(0.01, 999999)]
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
    }
}
