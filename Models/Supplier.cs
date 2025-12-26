using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, StringLength(300)]
        public required string Address { get; set; }

        [Required]
        public required string Phone { get; set; }
     
        public int FactoryId { get; set; }
        [ForeignKey(nameof(FactoryId))]
        public Factory? Factory { get; set; }

        public ICollection<MaterialPurchase>? MaterialPurchase { get; set; }
    }
}   
