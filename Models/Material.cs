using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        [Required]
        public string MaterialName { get; set; }

        public ICollection<MaterialPurchase> MaterialPurchase { get; set; }
    }
}
