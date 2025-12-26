using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        [Required]
        public required string Name { get; set; }
        public  string Unit { get; set; }
        [Required]
        public int FactoryId { get; set; }
        
        [ForeignKey(nameof(FactoryId))]
        public Factory? Factory { get; set; }

        public ICollection<MaterialPurchase>? MaterialPurchase { get; set; }
    }
}
