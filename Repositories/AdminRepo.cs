using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Models.DTOs.Admin;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class AdminRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;

        public AdminRepo(AppDbContext appDbContext, PasswordHasher passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
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
