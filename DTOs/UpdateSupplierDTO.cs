namespace FactoriesGateSystem.DTOs
{
    public class UpdateSupplierDTO
    {
        public required string Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public int CurrentBalance { get; set; }
    }
}
