using FactoriesGateSystem.Models.DTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IFactoryService
    {
        Task<List<FactoryDTO>> GetFactories(int? id, int? userId);
        Task<List<FactoryDTO>> GetFactoryByIdAsync(int id);
        Task<List<FactoryDTO>> GetFactoryNameAsync(string name);
    }
}
