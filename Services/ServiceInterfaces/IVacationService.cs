using FactoriesGateSystem.Models.DTOs.VacationDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IVacationService
    {
        Task<List<VacationDTO>> GetAllVacationAsync();
        Task<VacationDTO> GetVacationByIdAsync(int id);
        Task<VacationDTO> AddVacationAsync(CreateVacationDTO dto);
        Task<VacationDTO> UpdateVacationAsync(int id, UpdateVacationDTO dto);
        Task DeleteVacationAsync(int id);
    }
}
