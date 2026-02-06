using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly AppDbContext _appDbContext;

        public ManagerRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ManagerDTO>> GetManagersAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = _appDbContext.users.Where(u=>u.Role == "manager");
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(m => new ManagerDTO
            {
                Id = m.UserId,
                Name = m.Name,
                Email = m.Email,
                CreatedAt = m.CreatedAt,
            }).ToListAsync();
        }
    }
}
