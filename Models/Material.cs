using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        [Required]
        public string MaterialName { get; set; }
        [Required]
        public int MaterialQuantity { get; set; }
        [Required]
        public int MaterialPrice { get; set; }

        public ICollection<SupplierMaterial> SupplierMaterials { get; set; }
        public ICollection<EmployeeMaterial> EmployeeMaterials { get; set; }
    }
}
