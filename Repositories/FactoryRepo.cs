using Microsoft.EntityFrameworkCore;

namespace FactoriesGateSystem.Repositories
{
    public class FactoryRepo
    {
        private readonly AppDbContext _appDbContext;

        public FactoryRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> GetFactoryId(int userId)
        {
            var factory =await _appDbContext.factory.Where(f=> f.UserId == userId).FirstOrDefaultAsync();

            return factory!.FactoryId;
        }
    }
}
