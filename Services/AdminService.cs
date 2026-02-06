using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;
using System.Xml.Linq;

namespace FactoriesGateSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _adminRepo;

        public AdminService(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public async Task<List<AdminDTO>> GetAllAdminsAsync()
        {
            return await _adminRepo.GetAllAdminsAsync();
        }
        public async Task<List<AdminDTO>> GetAdminNameAsync(string name)
        {
            return await _adminRepo.GetAllAdminsAsync(a => a.Name.Contains(name)); 
        }
    }
}
