using FactoriesGateSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FactoriesGateSystem.Repositories
{
    public class AdminRepo
    {
        private readonly AppDbContext _appDbContext;

        public AdminRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<FactoryDTO>?> GetAllFactoriesAsync()
        {
            var facories = _appDbContext.factory;
            if(facories == null) { return null; }

            return await facories.Select(f => new FactoryDTO
            {
                Id = f.FactoryId,
                Name = f.Name,
                Address = f.Address,
            }).ToListAsync();
        }

        public async Task<List<ManagerDTO>?> GetAllManagersAsync()
        {
            var manager = _appDbContext.users.Where(m => m.Role == "manager");
            if (manager == null) { return null; }

            return await manager.Select(m => new ManagerDTO
            {
                Id = m.UserId,
                Name = m.Name,
                Email = m.Email,
                CreatedAt = m.CreatedAt,
            }).ToListAsync();
        }

        public async Task<FactoryDTO?> GetFactoryByIdAsync(int id)
        {
            var factory = await _appDbContext.factory.FindAsync(id);
            if (factory == null) { return null; }

            return new FactoryDTO
            {
                Id = factory.FactoryId,
                Name = factory.Name,
                Address = factory.Address,
            };
        }
    }
}
