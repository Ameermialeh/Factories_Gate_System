using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs
{
    public class FactoryDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public int ManagerId { get; set; }
    }
}
