using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Supplier : User
    {
        [Required]
        public string SupplierPhone { get; set; }

        public int CurrentBalance { get; set; } = 0;
        public override UserType access() => UserType.Supplier;
     

        public ICollection<SupplierMaterial> SupplierMaterials { get; set; }
    }
}   
