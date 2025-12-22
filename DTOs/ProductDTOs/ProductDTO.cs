using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }


        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
