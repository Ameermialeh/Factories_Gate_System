using FactoriesGateSystem.Models.DTOs.SupplierDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface ISupplierRepo
    {
        Task<List<SupplierDTO>> GetSupplierAsync(Expression<Func<Supplier, bool>>? filter = null);
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<SupplierDTO> AddSupplierAsync(CreateSupplierDTO supplierDto, int factoryId);
        Task<SupplierDTO?> UpdateSupplierAsync(int id, UpdateSupplierDTO dto);
        Task<bool> ChickIfSupplierExistAsync(int supplierId, int factoryId);
        Task<Supplier?> DeleteSupplierAsync(int id);
    }
}
