using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.OrderDTOs
{
    public class OrderWithProductsDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.MinValue;
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public ICollection<OrderItemDTO> Products { get; set; } = new List<OrderItemDTO>();
    }
}
