using FactoriesGateSystem.Models.DTOs.MaterialDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.AddMaterialDTO;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IMaterialRepo
    {
        Task<List<MaterialDTO>> GetMaterialAsync(Expression<Func<Material, bool>>? filter = null);
        Task<Material?> GetMaterialByIdAsync(int id);
        Task<MaterialDTO> AddNewMaterialAsync(AddNewMaterialDTO dto, int factoryId);
        Task<MaterialDTO> AddExistingMaterialAsync(AddExistingMaterialDTO dto);
        Task<bool> ChickIfMaterialExistAsync(int materialId, int factoryId);
        Task<bool> ChickIfMaterialNameExistAsync(string Name, int factoryId);
        Task<MaterialDTO?> UpdateMaterialAsync(int id, UpdateMaterialDTO dto, int factoryId);
        Task<bool> chickIfMaterialQuantityZeroAsync(int materialId, int factoryId);
        Task<bool> DeleteMaterialAsync(int id, int factoryId);
    }
}
