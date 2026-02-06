using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.AddMaterialDTO;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.UpdateMaterialDTO;
using FactoriesGateSystem.Models.DTOs.MaterialDTOs;
using FactoriesGateSystem.Repositories.Interfaces;

namespace FactoriesGateSystem.Repositories
{
    public class MaterialRepo : IMaterialRepo
    {
        private readonly AppDbContext _appDbContext;

        public MaterialRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<List<MaterialDTO>> GetMaterialAsync(Expression< Func<Material, bool>>? filter = null)
        {
            IQueryable<Material> query = _appDbContext.materials;
            if (filter != null) 
                query = query.Where(filter);


            return await query.Select(m => new MaterialDTO()
            {
                ID = m.MaterialId,
                Name = m.Name,
                Quantity = m.Inventory!.Quantity
            }).ToListAsync();
        }

        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
              return await _appDbContext.materials.Include(m => m.Inventory).FirstOrDefaultAsync(m => m.MaterialId == id);
        }

        public async Task<MaterialDTO> AddNewMaterialAsync(AddNewMaterialDTO dto, int factoryId)
        {

            InventoryMaterial inventory = new InventoryMaterial
            {
                Quantity = dto.Quantity,
                LastUpdated = DateTime.UtcNow.Date,
            };
            await _appDbContext.inventoryMaterials.AddAsync(inventory);
            await _appDbContext.SaveChangesAsync();
            Material material = new Material
            {
                Name = dto.Name!.Trim(),
                FactoryId = factoryId,
                InventoryId = inventory.InventoryId,
            };
            await _appDbContext.materials.AddAsync(material);
            await _appDbContext.SaveChangesAsync();

            var materialPurchase = new MaterialPurchase
            {
                SupplierId = dto.SupplierId,
                MaterialId = material.MaterialId,
                PricePerUnit = dto.PricePerUnit,
                Date = DateTime.UtcNow.Date,
                Quantity = dto.Quantity,
            };
            await _appDbContext.MaterialPurchase.AddAsync(materialPurchase);
            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.Name,
                Quantity = inventory.Quantity,
            };
        }

        public async Task<MaterialDTO> AddExistingMaterialAsync(AddExistingMaterialDTO dto)
        {
            var material = await _appDbContext.materials.Include(m => m.Inventory).FirstOrDefaultAsync(m => m.MaterialId == dto.MaterialId);

            var materialPurchase = new MaterialPurchase
            {
                SupplierId = dto.SupplierId,
                MaterialId = dto.MaterialId,
                PricePerUnit = dto.PricePerUnit,
                Date = DateTime.UtcNow.Date,
                Quantity = dto.Quantity,
            };
            await _appDbContext.MaterialPurchase.AddAsync(materialPurchase);

            material!.Inventory!.Quantity += dto.Quantity;
            material.Inventory.LastUpdated = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.Name,
                Quantity = material.Inventory!.Quantity
            };
        }

        public async Task<bool> ChickIfMaterialExistAsync(int materialId, int factoryId)
        {
           return await _appDbContext.materials.AnyAsync(m => m.MaterialId == materialId && m.FactoryId == factoryId);
        }

        public async Task<bool> ChickIfMaterialNameExistAsync(string Name, int factoryId)
        {
            return await _appDbContext.materials.AnyAsync(m => m.Name == Name && m.FactoryId == factoryId);
        }

        public async Task<MaterialDTO?> UpdateMaterialAsync(int id, UpdateMaterialDTO dto, int factoryId)
        {
            var material = await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == id && m.FactoryId == factoryId);
            if (material == null) return null;

            if (dto.Name != null)
                material.Name = dto.Name;

            if (dto.Quantity != null && material.Inventory != null)
            {
                material.Inventory.Quantity = dto.Quantity.Value;
                material.Inventory.LastUpdated = DateTime.UtcNow;
            }

            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.Name,
                Quantity = material.Inventory?.Quantity ?? 0
            };
        }

        public async Task<bool> chickIfMaterialQuantityZeroAsync(int materialId, int factoryId)
        {
            return await _appDbContext.materials.Where(m =>
            m.MaterialId == materialId &&
            m.FactoryId == factoryId).AnyAsync(m => m.Inventory != null && m.Inventory.Quantity == 0);
        }

        public async Task<bool> DeleteMaterialAsync(int id, int factoryId)
        {
            var material = await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == id && m.FactoryId == factoryId);
            if (material == null) return false;

            var inventory = await _appDbContext.inventoryMaterials.FirstOrDefaultAsync(i => i.InventoryId == material.InventoryId);

            _appDbContext.materials.Remove(material);
            _appDbContext.inventoryMaterials.Remove(inventory!);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
