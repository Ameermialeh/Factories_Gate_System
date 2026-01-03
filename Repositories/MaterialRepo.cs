using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static FactoriesGateSystem.DTOs.MaterialDTOs.AddMaterialDTO;
using static FactoriesGateSystem.DTOs.MaterialDTOs.UpdateMaterialDTO;

namespace FactoriesGateSystem.Repositories
{
    public class MaterialRepo
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
            }).ToListAsync();
        }

        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == id);
        }

        public async Task<MaterialDTO> AddNewMaterialAsync(AddNewMaterialDTO dto, int factoryId)
        {

            var existingMaterial = await _appDbContext.materials.FirstOrDefaultAsync(m => m.Name == dto.Name && m.FactoryId == factoryId && m.IsDeleted);
            Material material;

            if (existingMaterial != null)
            {
                existingMaterial.IsDeleted = false;
                material = existingMaterial;
            }
            else
            {
                material = new Material
                {
                    Name = dto.Name!.Trim(),
                    FactoryId = factoryId
                };

                await _appDbContext.materials.AddAsync(material);
                await _appDbContext.SaveChangesAsync();
            }

            var materialPurchase = new MaterialPurchase
            {
                SupplierId = dto.SupplierId,
                MaterialId = material.MaterialId,
                PricePerUnit = dto.PricePerUnit,
                Date = DateTime.UtcNow.Date,
                Quantity = dto.Quantity,
            };
            await _appDbContext.MaterialPurchase.AddAsync(materialPurchase);

            var inventory = await _appDbContext.inventories.FirstOrDefaultAsync(i => i.MaterialId == material.MaterialId);
            if (inventory != null)
            {
                inventory.Quantity += dto.Quantity;
                inventory.LastUpdated = DateTime.UtcNow;
                inventory.IsDeleted = false;
            }
            else
            {
                inventory = new Inventory
                {
                    MaterialId = material.MaterialId,
                    Quantity = dto.Quantity,
                    LastUpdated = DateTime.UtcNow.Date,
                };
                await _appDbContext.inventories.AddAsync(inventory);
            }

            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.Name,
                Quantity = inventory.Quantity,
            };
        }

        public async Task<MaterialDTO?> AddExistingMaterialAsync(AddExistingMaterialDTO dto)
        {

            var materialPurchase = new MaterialPurchase
            {
                SupplierId = dto.SupplierId,
                MaterialId = dto.MaterialId,
                PricePerUnit = dto.PricePerUnit,
                Date = DateTime.UtcNow.Date,
                Quantity = dto.Quantity,
            };
            await _appDbContext.MaterialPurchase.AddAsync(materialPurchase);

            var inventory = await _appDbContext.inventories.Include(i=>i.Material).FirstOrDefaultAsync(i=> i.MaterialId == dto.MaterialId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    MaterialId = dto.MaterialId,
                    Quantity = dto.Quantity,
                    LastUpdated = DateTime.UtcNow
                };

                await _appDbContext.inventories.AddAsync(inventory);
            }
            else
            {
                inventory.Quantity += dto.Quantity;
                inventory.LastUpdated = DateTime.UtcNow;
            }
     
            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO()
            {
                ID = inventory.MaterialId,
                Name = inventory.Material!.Name,
                Quantity = inventory.Quantity
            };
        }

        public async Task<bool> ChickIfMaterialExistAsync(int materialId, int factoryId)
        {
           return await _appDbContext.materials.AnyAsync(m => m.MaterialId == materialId && m.FactoryId == factoryId && !m.IsDeleted);
        }

        public async Task<bool> ChickIfMaterialNameExistAsync(string Name, int factoryId)
        {
            return await _appDbContext.materials.AnyAsync(m => m.Name == Name && m.FactoryId == factoryId && !m.IsDeleted);
        }

        public async Task<MaterialDTO?> UpdateMaterialNameAsync(UpdateNameMaterialDTO dto,int factoryId)
        {
            var material = await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == dto.Id && m.FactoryId == factoryId);
            if (material == null) return null;

            material.Name = dto.Name!;
            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.Name,
            };
        }

        public async Task<MaterialDTO?> UpdateMaterialQuantityAsync(UpdateQuantityMaterialDTO dto,int factoryId)
        {
            var inventory = await _appDbContext.inventories.Include(i => i.Material)
                .FirstOrDefaultAsync(i=>i.MaterialId == dto.Id && i.Material!.FactoryId == factoryId);

            if(inventory == null) return null;

            inventory.Quantity = dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();

            return new MaterialDTO
            {
                ID = inventory.MaterialId,
                Name = inventory.Material!.Name,
                Quantity = inventory.Quantity,
            };
        }

        public async Task<bool> chickIfMaterialQuantityZeroAsync(int materialId, int factoryId)
        {
            return await _appDbContext.inventories
                .AnyAsync(i =>
                    i.MaterialId == materialId &&
                    i.Quantity == 0 &&
                    i.Material!.FactoryId == factoryId &&
                    !i.Material.IsDeleted);
        }

        public async Task<bool> DeleteMaterialAsync(int id, int factoryId)
        {
            var material = await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == id && m.FactoryId == factoryId);
            if (material == null) return false;

            material.IsDeleted = true;

            var inventory = await _appDbContext.inventories.FirstOrDefaultAsync(i => i.MaterialId == id);
            if(inventory != null)
            {
                inventory.IsDeleted = true;
                inventory.LastUpdated = DateTime.UtcNow;
            }
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
