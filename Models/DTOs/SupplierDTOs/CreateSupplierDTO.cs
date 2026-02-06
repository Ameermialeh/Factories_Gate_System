using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.SupplierDTOs
{
    public class CreateSupplierDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
