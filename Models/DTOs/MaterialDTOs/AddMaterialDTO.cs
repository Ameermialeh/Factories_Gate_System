using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.MaterialDTOs
{
    public class AddMaterialDTO
    {
        public class AddNewMaterialDTO
        {
            [Required]
            public string Name { get; set; } = string.Empty;
            [Required]
            public int SupplierId { get; set; }
            [Required]
            public decimal PricePerUnit { get; set; }
            [Required]
            public int Quantity { get; set; }
        }

        public class AddExistingMaterialDTO
        {
            [Required]
            public int MaterialId { get; set; }
            [Required]
            public int SupplierId { get; set; }
            [Required]
            public decimal PricePerUnit { get; set; }
            [Required]
            public int Quantity { get; set; }
        }
    }
}
