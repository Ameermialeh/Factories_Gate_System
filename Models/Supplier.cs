using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(100)]
        public required string SupplierName { get; set; }

        [Required, StringLength(300)]
        public required string Address { get; set; }

        [Required]
        public required string SupplierPhone { get; set; }

        public int CurrentBalance { get; set; } = 0;

        public UserType access() => UserType.Supplier;
     

        public ICollection<MaterialPurchase>? MaterialPurchase { get; set; }
    }
}   
