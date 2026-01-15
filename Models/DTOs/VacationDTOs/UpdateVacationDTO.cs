namespace FactoriesGateSystem.Models.DTOs.VacationDTOs
{
    public class UpdateVacationDTO
    {
        public class UpdateVacationDate
        {
            public int VacationId { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
        }

        public class UpdateVacationReasone
        {
            public int VacationId { get; set; }
            public string? VacationReason { get; set; }
        }
    }
}
