namespace FactoriesGateSystem.Models.DTOs.OrderDTOs
{
    public class OrderWithProductsDTO
    {

        public string? Name { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? CustomerID { get; set; }

        public ICollection<OrderItemDTO>? Products { get; set; }
    }
}
