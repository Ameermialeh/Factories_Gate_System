using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class MaterialPurchase
    {
        [Key]
        public int MaterialPurchaseId { get; set; }

        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int MaterialId { get; set; }

        [Range(0.01, 999999)]
        public decimal PricePerUnit { get; set; }
        public DateTime? Date { get; set; } = DateTime.UtcNow;

        [Required]
        public int Quantity { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier? Supplier { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public Material? Material { get; set; }


    }
}
