using FactoriesGateSystem.Models.DTOs.SupplierDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierDTO>> GetAllSuppliers();
        Task<List<SupplierDTO>> GetAllSuppliersWithNameAsync(string name);
        Task<SupplierDTO> GetSupplierByIdAsync(int id);
        Task<List<SupplierDTO>> GetSuppliersByNameAsync(string name);
        Task<SupplierDTO> AddSupplierAsync(CreateSupplierDTO dto);
        Task<SupplierDTO> UpdateSupplierAsync(int id, UpdateSupplierDTO dto);
        Task<DeleteSupplierDTO> DeleteSupplierAsync(int id);
    }
}
