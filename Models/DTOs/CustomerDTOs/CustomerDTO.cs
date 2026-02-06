namespace FactoriesGateSystem.Models.DTOs.CustomerDTOs
{
    public class CustomerDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
