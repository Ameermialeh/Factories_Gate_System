using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IAdminRepo
    {
        Task<List<AdminDTO>> GetAllAdminsAsync(Expression<Func<User, bool>>? filter = null);
    }
}
