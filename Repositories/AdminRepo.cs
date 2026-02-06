using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class AdminRepo: IAdminRepo
    {
        private readonly AppDbContext _appDbContext;

        public AdminRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<AdminDTO>> GetAllAdminsAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = _appDbContext.users.Where(u => u.Role == "admin");
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(a => new AdminDTO
            {
                Id = a.UserId,
                Name = a.Name,
                Email= a.Email,
                CreatedAt = a.CreatedAt,
            }).ToListAsync();
        }
    }
}
