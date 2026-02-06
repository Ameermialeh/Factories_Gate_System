using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models.DTOs.VacationDTOs
{
    public class CreateVacationDTO
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public string VacationReason { get; set; } = string.Empty;
    }
}
