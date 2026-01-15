namespace FactoriesGateSystem.Models.DTOs.VacationDTOs
{
    public class CreateVacationDTO
    {
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string? VacationReason { get; set; }
    }
}
