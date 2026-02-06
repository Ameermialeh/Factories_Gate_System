using FactoriesGateSystem.Models.DTOs.EmployeeDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<List<EmployeeDTO>> GetEmployeeNameAsync(string name);
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO dto);
        Task<EmployeeDTO> UpdateEmployeeAsync(int id, UpdateEmployeeDTO dto);
        Task DeleteEmployeeAsync(int id);
    }
}
