using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.DTOs
{
    public class VacationDTO
    {
        public int VacationId { get; set; }
       
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string? VacationReason { get; set; }
    }
}
