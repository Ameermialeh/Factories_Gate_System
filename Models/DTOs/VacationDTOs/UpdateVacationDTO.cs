namespace FactoriesGateSystem.Models.DTOs.VacationDTOs
{
    public class UpdateVacationDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? VacationReason { get; set; }
    }
}
