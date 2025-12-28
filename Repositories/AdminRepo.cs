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
    }
}
