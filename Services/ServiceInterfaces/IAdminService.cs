using FactoriesGateSystem.Models.DTOs.Admin;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IAdminService
    {
        Task<List<AdminDTO>> GetAllAdminsAsync();
        Task<List<AdminDTO>> GetAdminNameAsync(string name);
    }
}
