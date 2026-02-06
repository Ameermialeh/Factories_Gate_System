using FactoriesGateSystem.Models.DTOs.Admin;
using static FactoriesGateSystem.Models.DTOs.AuthDTO;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IAuthService
    {
        Task CreateAdminAsync(RegisterAdminDTO dto);
        Task CreateUserAsync(RegisterDTO dto);
        Task<string> LoginUserAsync(LoginDTO dto);
        Task LogoutAsync();
        Task<string> RefreshAsync(RefreshTokenDTO dto);
        Task ChangePasswordAsync(ChangePasswordDTO dto);
    }
}
