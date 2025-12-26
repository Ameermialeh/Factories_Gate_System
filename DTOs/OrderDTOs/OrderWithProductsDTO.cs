namespace FactoriesGateSystem.DTOs.OrderDTOs
{
    public class OrderWithProductsDTO
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public DateTime OrderDate { get; set; }

        public int CustomerID { get; set; }

        public int FactoryId { get; set; }
        public ICollection<OrderProductsDTO>? Products { get; set; }
    }
}
