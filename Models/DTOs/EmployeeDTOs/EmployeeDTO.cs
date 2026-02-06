using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.EmployeeDTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
