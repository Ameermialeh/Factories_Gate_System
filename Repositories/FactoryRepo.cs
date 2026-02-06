using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class FactoryRepo : IFactoryRepo
    {
        private readonly AppDbContext _appDbContext;

        public FactoryRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<FactoryDTO>> GetFactoryAsync(Expression<Func<Factory, bool>>? filter = null)
        {
            IQueryable<Factory> query = _appDbContext.factory;
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(f => new FactoryDTO
            {
                Id = f.FactoryId,
                Name = f.Name,
                Address = f.Address,
                ManagerId = f.UserId
            }).ToListAsync();
        }

        public async Task<int> GetFactoryId(int userId)
        {
            var factory =await _appDbContext.factory.Where(f=> f.UserId == userId).FirstOrDefaultAsync();

            return factory!.FactoryId;
        }
    }
}
