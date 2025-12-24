using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }

        public required string SupplierName { get; set; }

        public string? Address { get; set; }

        public string? SupplierPhone { get; set; }

        public int? CurrentBalance { get; set; }
    }
}
