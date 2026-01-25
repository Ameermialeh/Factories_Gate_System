namespace FactoriesGateSystem.Models.DTOs.ProductDTOs
{
    public class ProductResponseDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
