using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Suppliers
    {
        [Key]
        public int SuppliersId { get; set; }
        [Required]
        public string SupplierName { get; set; }
        [Required]
        public string SupplierPhone { get; set; }

        public int CurrentBalance { get; set; } = 0;
    }
}
