using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs.EmployeeDTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}
