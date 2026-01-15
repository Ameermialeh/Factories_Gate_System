using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.MaterialDTOs
{
    public class MaterialDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public int Quantity { get; set; }
    }
}
