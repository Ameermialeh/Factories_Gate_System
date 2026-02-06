using FactoriesGateSystem.Models.DTOs.MaterialDTOs;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.AddMaterialDTO;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IMaterialService
    {
        Task<List<MaterialDTO>> GetAllMaterials();
        Task<List<MaterialDTO>> GetAllMaterialsWithNameAsync(string name);
        Task<MaterialDTO> GetMaterialByIdAsync(int id);
        Task<List<MaterialDTO>> GetMaterialByNameAsync(string name);
        Task<MaterialDTO> AddNewMaterialAsync(AddNewMaterialDTO dto);
        Task<MaterialDTO> AddExistingMaterialAsync(AddExistingMaterialDTO dto);
        Task<MaterialDTO> UpdateMaterialAsync(int id, UpdateMaterialDTO dto);
        Task DeleteMaterialAsync(int id);
    }
}
