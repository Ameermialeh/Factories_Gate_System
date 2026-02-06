using FactoriesGateSystem.Models.DTOs.EmployeeDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<List<EmployeeDTO>> GetEmployeesAsync(Expression<Func<Employee, bool>>? filter = null);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDto, int factoryId);
        Task<EmployeeDTO?> UpdateEmployeeAsync(int id, UpdateEmployeeDTO dto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
