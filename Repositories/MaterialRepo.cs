using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                Unit = m.Unit,
            }).ToListAsync();
        }

        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == id);
        }

        public async Task<Material> CreateMaterialAsync(CreateMaterialDTO dto)
        {
            var material = new Material()
            {
                Name = dto.Name!,
                Unit = dto.Unit!
            };

            await _appDbContext.materials.AddAsync(material);
            await _appDbContext.SaveChangesAsync();

            return material;
        }

        public async Task<Material?> UpdateMaterialAsync(int id, CreateMaterialDTO dto)
        {
            var material =await _appDbContext.materials.FindAsync(id);
            if (material == null) return null;

            material.Name = dto.Name!;
            material.Unit = dto.Unit!;
            await _appDbContext.SaveChangesAsync();

            return material;
        }

        public async Task<Material?> DeleteMaterialAsync(int id)
        {
            var material = await _appDbContext.materials.FindAsync(id);
            if (material == null) return null;

            _appDbContext.materials.Remove(material);
            await _appDbContext.SaveChangesAsync();
            return material;
        }
    }
}
