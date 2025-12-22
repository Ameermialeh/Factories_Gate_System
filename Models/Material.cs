using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        [Required]
        public required string MaterialName { get; set; }

        public bool Hide { get; set; } = false;
        public ICollection<MaterialPurchase>? MaterialPurchase { get; set; }
    }
}
