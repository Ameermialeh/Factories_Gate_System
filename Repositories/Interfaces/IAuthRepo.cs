using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.Admin;
using static FactoriesGateSystem.Models.DTOs.AuthDTO;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IAuthRepo
    {
        Task RegisterAsync(RegisterDTO dto, string passwordHash);
        Task<User?> LoginAsync(LoginDTO dto);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<User?> GetUserByRefreshTokenAsync(string token);
        Task<bool> RevokeRefreshTokenAsync(string token);
        Task<User> RegisterAdminAsync(RegisterAdminDTO dto, string passwordHash);
        Task<bool> LogoutAsync(int userId);
        Task<User?> getAUserByEmailAsync(string email);
        bool PasswordValid(User user, ChangePasswordDTO dto);
        Task UpdatePasswordAsync(User user, ChangePasswordDTO dto);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> FactoryNameExistsAsync(string name);
    }
}
