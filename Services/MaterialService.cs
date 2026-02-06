using FactoriesGateSystem.Models.DTOs.MaterialDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.AddMaterialDTO;

namespace FactoriesGateSystem.Services
{
    public class MaterialService: IMaterialService
    {
        private readonly IMaterialRepo _materialRepo;
        private readonly ISupplierRepo _supplierRepo;
        private readonly ICookieService _cookieService;
        public MaterialService(IMaterialRepo materialRepo, ISupplierRepo supplierRepo, ICookieService cookieService)
        {
            _materialRepo = materialRepo;
            _supplierRepo = supplierRepo;
            _cookieService = cookieService;
        }

        public async Task<List<MaterialDTO>> GetAllMaterials()
        {
            var material = await _materialRepo.GetMaterialAsync();
            return material;
        }
        public async Task<List<MaterialDTO>> GetAllMaterialsWithNameAsync(string name)
        {
            var filtered = await _materialRepo.GetMaterialAsync(m => m.Name.Contains(name));
            return filtered;
        }
        public async Task<MaterialDTO> GetMaterialByIdAsync(int id)
        {
            var material = await _materialRepo.GetMaterialByIdAsync(id)
                ?? throw new BadHttpRequestException("Material not found", StatusCodes.Status404NotFound);

            var materialdto = new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.Name,
                Quantity = material.Inventory!.Quantity
            };
            return materialdto;
        }
        public async Task<List<MaterialDTO>> GetMaterialByNameAsync(string name)
        {
            var material = await _materialRepo.GetMaterialAsync(m => m.Name.Contains(name));
            return material;
        }
        public async Task<MaterialDTO> AddNewMaterialAsync(AddNewMaterialDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var supplier = await _supplierRepo.ChickIfSupplierExistAsync(dto.SupplierId, int.Parse(factoryId));
            
            if (!supplier) throw new BadHttpRequestException("Supplier not found.", StatusCodes.Status404NotFound);

            var Name = await _materialRepo.ChickIfMaterialNameExistAsync(dto.Name, int.Parse(factoryId));
            if (Name) throw new BadHttpRequestException("Material Name already exists.", StatusCodes.Status409Conflict);

            var materialDto = await _materialRepo.AddNewMaterialAsync(dto, int.Parse(factoryId));
            return materialDto;
        }
        public async Task<MaterialDTO> AddExistingMaterialAsync(AddExistingMaterialDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
                 ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var materialExists = await _materialRepo.ChickIfMaterialExistAsync(dto.MaterialId, int.Parse(factoryId));
            if (!materialExists) throw new BadHttpRequestException("Material not found.", StatusCodes.Status404NotFound);


            var supplierExists = await _supplierRepo.ChickIfSupplierExistAsync(dto.SupplierId, int.Parse(factoryId));
            if (!supplierExists) throw new BadHttpRequestException("Supplier not found.", StatusCodes.Status404NotFound);


            var material = await _materialRepo.AddExistingMaterialAsync(dto);
            return material;
        }
        public async Task<MaterialDTO> UpdateMaterialAsync(int id, UpdateMaterialDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
                 ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            if (dto.Name != null)
            {
                var nameExists = await _materialRepo
                    .ChickIfMaterialNameExistAsync(dto.Name, int.Parse(factoryId));

                if (!nameExists)
                    throw new BadHttpRequestException("Material Name already exists.", StatusCodes.Status409Conflict);
            }
            var material = await _materialRepo.UpdateMaterialAsync(id, dto, int.Parse(factoryId))
                ?? throw new BadHttpRequestException("Material not found.", StatusCodes.Status404NotFound);

            return material;
        }
        public async Task DeleteMaterialAsync(int id)
        {
            var factoryId = _cookieService.Get("FactoryId")
                  ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var IsZero = await _materialRepo.chickIfMaterialQuantityZeroAsync(id, int.Parse(factoryId));
            if (!IsZero) throw new BadHttpRequestException("Cannot delete material because the quantity is not zero.", StatusCodes.Status400BadRequest);  

            var material = await _materialRepo.DeleteMaterialAsync(id, int.Parse(factoryId));
            if (!material) throw new BadHttpRequestException("Material not found.", StatusCodes.Status404NotFound);
        }
    }
}
