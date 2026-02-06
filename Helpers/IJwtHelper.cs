using FactoriesGateSystem.Models;

namespace FactoriesGateSystem.Helpers
{
    public interface IJwtHelper
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken();
    }
}
