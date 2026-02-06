using FactoriesGateSystem.Models.DTOs.SalaryDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface ISalaryRepo
    {
        Task<List<SalaryDTO>> GetAllSalariesAsync(Expression<Func<Salary, bool>>? filter = null);
        Task<Salary?> GetSalaryByIdAsync(int id);
        Task<SalaryDTO> AddSalaryForEmployeeAsync(AddSalaryDTO dto);
        Task<SalaryDTO?> UpdateSalariesAsync(int id, UpdateSalaryDTO dto);
        Task<bool> DeleteSalaryAsync(int id);
    }
}
