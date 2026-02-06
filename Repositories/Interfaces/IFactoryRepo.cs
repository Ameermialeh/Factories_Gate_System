using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IFactoryRepo
    {
        Task<List<FactoryDTO>> GetFactoryAsync(Expression<Func<Factory, bool>>? filter = null);
        Task<int> GetFactoryId(int userId);
    }
}
