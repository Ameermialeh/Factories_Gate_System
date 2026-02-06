using FactoriesGateSystem.Models.DTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IManagerService
    {
        Task<List<ManagerDTO>> GetAllManagersAsync();
        Task<List<ManagerDTO>> GetAllManagersWithNameAsync(string name);
        Task<List<ManagerDTO>> GetManagerByIdAsync(int id);
    }
}
