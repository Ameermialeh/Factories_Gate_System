using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using static FactoriesGateSystem.DTOs.AuthDTO;

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
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "manager",
                CreatedAt = DateTime.UtcNow
            };

            await _appDbContext.users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            var factory = new Factory
            {
                Name = dto.FactoryName,
                Address = dto.Address,
                UserId = user.UserId
            };

            await _appDbContext.factory.AddAsync(factory);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(LoginDTO dto)
        {
            var user = await _appDbContext.users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return null;

            bool ph = _passwordHasher.Verify(dto.Password, user.PasswordHash);
            if (!ph) return null;
            return user;
        }


        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _appDbContext.refreshtokens.Add(refreshToken);
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
    }
}
