using FactoriesGateSystem.Models.DTOs.VacationDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IVacationRepo
    {
        Task<List<VacationDTO>> GetAllVacationAsync(Expression<Func<Vacation, bool>>? filter = null);
        Task<Vacation?> GetVacationByIdAsync(int id);
        Task<VacationDTO> AddVacationToEmployee(CreateVacationDTO dto);
        Task<VacationDTO?> UpdateVacationAsync(int id, UpdateVacationDTO dto);
        Task<bool> DeleteVacationAsync(int id);
    }
}
