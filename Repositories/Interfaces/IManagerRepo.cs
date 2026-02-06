using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IManagerRepo
    {
        Task<List<ManagerDTO>> GetManagersAsync(Expression<Func<User, bool>>? filter = null);
    }
}
