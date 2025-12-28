using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs
{
    public class ManagerDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
