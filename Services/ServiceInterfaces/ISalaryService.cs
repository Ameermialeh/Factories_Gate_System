using FactoriesGateSystem.Models.DTOs.SalaryDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface ISalaryService
    {
        Task<List<SalaryDTO>> GetAllSalariesAsync();
        Task<List<SalaryDTO>> GetAllSalariesByEmployeeId(int employeeId);
        Task<SalaryDTO> GetSalaryByIdAsync(int id);
        Task<List<SalaryDTO>> GetAllSalariesInDateRangeAsync(int employeeId, DateTime FromDate, DateTime ToDate);
        Task<SalaryDTO> AddSalaryAsync(AddSalaryDTO dto);
        Task<SalaryDTO> UpdateSalaryAsync(int id, UpdateSalaryDTO dto);
        Task DeleteSalaryAsync(int id);
    }
}
