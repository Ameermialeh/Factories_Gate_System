using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(100)]
        public string SupplierName { get; set; }

        [Required, StringLength(300)]
        public string Address { get; set; }

        public DateTime? CreateAt { get; set; }

        [Required]
        public string SupplierPhone { get; set; }

        public int CurrentBalance { get; set; } = 0;

        public UserType access() => UserType.Supplier;
     

        public ICollection<SupplierMaterial> SupplierMaterials { get; set; }
    }
}   
