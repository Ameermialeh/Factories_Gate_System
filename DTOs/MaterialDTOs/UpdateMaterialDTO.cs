namespace FactoriesGateSystem.DTOs.MaterialDTOs
{
    public class UpdateMaterialDTO
    {
        public class UpdateNameMaterialDTO
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        public class  UpdateQuantityMaterialDTO
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }
    }
}
