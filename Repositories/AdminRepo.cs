using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.Admin;
using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ManagerDTO?> GetManagerByIdAsync(int id)
        {
            var manager =await _appDbContext.users.Where(m => m.UserId == id && m.Role == "manager").FirstOrDefaultAsync();
            if (manager == null) { return null; };

            return new ManagerDTO
            {
                Id = manager.UserId,
                Name = manager.Name,
                Email = manager.Email,
                CreatedAt = manager.CreatedAt,
            };
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

        public async Task<List<AdminDTO>> GetAllAdminsAsync()
        {
            var admins = _appDbContext.users.Where(a=>a.Role == "admin");
            return await admins.Select(a => new AdminDTO
            {
                Id = a.UserId,
                Name = a.Name,
                Email= a.Email,
                CreatedAt = a.CreatedAt,
            }).ToListAsync();
        }


        public async Task<User?> getAdminByEmailAsync(string email)
        {
            return await _appDbContext.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public bool PasswordValid(User user,ChangePasswordDTO dto)
        {
            return _passwordHasher.Verify(dto.CurrentPassword!, user.PasswordHash);
        }

        public async Task UpdatePasswordAsync(User user, ChangePasswordDTO dto)
        {
            user.PasswordHash = _passwordHasher.Hash(dto.NewPassword!);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
