using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.Admin;
using Microsoft.EntityFrameworkCore;
using static FactoriesGateSystem.Models.DTOs.AuthDTO;

namespace FactoriesGateSystem.Repositories
{
    public class AuthRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;
        public AuthRepo(AppDbContext appDbContext, PasswordHasher passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> RegisterAsync(RegisterDTO dto, string passwordHash)
        {
            var user = new User
            {
                Name = dto.Name!,
                Email = dto.Email!,
                PasswordHash = passwordHash,
                Role = "manager",
                CreatedAt = DateTime.UtcNow.Date,
            };

            await _appDbContext.users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            var factory = new Factory
            {
                Name = dto.FactoryName!,
                Address = dto.Address,
                UserId = user.UserId
            };

            await _appDbContext.factory.AddAsync(factory);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(LoginDTO dto)
        {
            return await _appDbContext.users.FirstOrDefaultAsync(u => u.Email == dto.Email);  
        }


        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _appDbContext.refreshtokens.AddAsync(refreshToken);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string token)
        {
            var refreshToken = await _appDbContext.refreshtokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r =>
                    r.Token == token &&
                    !r.IsRevoked &&
                    r.ExpiresAt > DateTime.UtcNow);

            return refreshToken?.User;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _appDbContext.refreshtokens
                .FirstOrDefaultAsync(r => r.Token == token);

            if (refreshToken == null)
                return false;

            refreshToken.IsRevoked = true;
            await _appDbContext.SaveChangesAsync();
            return true;
        }


        public async Task<User> RegisterAdminAsync(RegisterAdminDTO dto, string passwordHash)
        {
            var user = new User
            {
                Name = dto.Name!,
                Email = dto.Email!,
                PasswordHash = passwordHash,
                Role = "admin",
                CreatedAt = DateTime.UtcNow.Date,
            };

            await _appDbContext.users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }


        public async Task<bool> LogoutAsync(int userId)
        {
            var refreshToken =await _appDbContext.refreshtokens.Where(rt => rt.UserId == userId).OrderByDescending(rt=> rt.Id).FirstOrDefaultAsync();
            if(refreshToken == null) return false;

            refreshToken.IsRevoked = true;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User?> getAUserByEmailAsync(string email)
        {
            return await _appDbContext.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public bool PasswordValid(User user, ChangePasswordDTO dto)
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
