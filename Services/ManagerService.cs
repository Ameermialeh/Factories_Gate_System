using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepo _managerRepo;

        public ManagerService (IManagerRepo managerRepo)
        {
            _managerRepo = managerRepo;
        }

        public async Task<List<ManagerDTO>> GetAllManagersAsync()
        {
            var managers = await _managerRepo.GetManagersAsync();
            return managers;
        }

        public async Task<List<ManagerDTO>> GetAllManagersWithNameAsync(string name)
        {
            var filtered = await _managerRepo.GetManagersAsync(m => m.Name.Contains(name));
            return filtered;
        }
        public async Task<List<ManagerDTO>> GetManagerByIdAsync(int id)
        {
            var manager = await _managerRepo.GetManagersAsync(m => m.UserId == id);
            if (manager.Count == 0) { throw new BadHttpRequestException("Manager not found", StatusCodes.Status404NotFound); }
            return manager;
        }
    }
}
