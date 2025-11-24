using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Materials
    {
        [Key]
        public int MaterialsId { get; set; }
        [Required]
        public string MaterialsName { get; set; }
        [Required]
        public int MaterialsQuantity { get; set; }
        [Required]
        public int MaterialsPrice { get; set; }
    }
}
