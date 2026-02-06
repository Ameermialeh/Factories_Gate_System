using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string NameAr { get; set; } = string.Empty;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; } 
    }
}
