namespace FactoriesGateSystem.DTOs
{
    public class OrderWithProductsDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime OrderDate { get; set; }

        public string Description { get; set; }

        public int CustomerID { get; set; }

        public ICollection<OrderProductsDTO> Products { get; set; }
    }
}
