namespace FactoriesGateSystem.DTOs.MaterialDTOs
{
    public class AddMaterialDTO
    {
        public class AddNewMaterialDTO
        {
            public string? Name { get; set; }

            public int SupplierId { get; set; }

            public decimal PricePerUnit { get; set; }

            public int Quantity {  get; set; }
        }

        public class AddExistingMaterialDTO
        {
            public int MaterialId { get; set; }

            public int SupplierId { get; set; }

            public decimal PricePerUnit { get; set; }

            public int Quantity { get; set; }
        }
    }
}
