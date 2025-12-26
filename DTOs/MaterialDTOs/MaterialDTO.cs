using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs.MaterialDTOs
{
    public class MaterialDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public string? Unit { get; set; }
    }
}
