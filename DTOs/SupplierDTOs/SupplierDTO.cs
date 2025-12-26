using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs.SupplierDTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
    }
}
