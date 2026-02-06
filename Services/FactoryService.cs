using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class FactoryService : IFactoryService
    {
        private readonly IFactoryRepo _factoryRepo;

        public FactoryService(IFactoryRepo factoryRepo)
        {
            _factoryRepo = factoryRepo;
        }
        public async Task<List<FactoryDTO>> GetFactories(int? id, int? userId)
        {
            var factories = await _factoryRepo.GetFactoryAsync(f => (id == null || f.FactoryId == id.Value) && (userId == null || f.UserId == userId.Value));

            if (factories.Count == 0)
                 throw new BadHttpRequestException("No Factories found", StatusCodes.Status404NotFound);
            return factories;
        }
        public async Task<List<FactoryDTO>> GetFactoryByIdAsync(int id)
        {
            var factory = await _factoryRepo.GetFactoryAsync(f => f.FactoryId == id)
                ?? throw new BadHttpRequestException("Factory not found", StatusCodes.Status404NotFound);

            return factory;
        }
        public async Task<List<FactoryDTO>> GetFactoryNameAsync(string name)
        {
            var factories = await _factoryRepo.GetFactoryAsync(f => f.Name.Contains(name));

            if (factories.Count == 0)
                throw new BadHttpRequestException("No Factories found", StatusCodes.Status404NotFound);

            return factories;
        }
    }
}
