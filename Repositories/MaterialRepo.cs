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
            IQueryable<Material> query = _appDbContext.materials.Where(m=> !m.Hide);
            if (filter != null) 
                query = query.Where(filter);
            return await query.Select(m => new MaterialDTO()
            {
                ID = m.MaterialId,
                Name = m.MaterialName,
            }).ToListAsync();
        }

        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await _appDbContext.materials.FirstOrDefaultAsync(m => m.MaterialId == id && !m.Hide);
        }

        public async Task<Material> CreateMaterialAsync(string name)
        {
            var material = new Material()
            {
                MaterialName = name
            };

            await _appDbContext.materials.AddAsync(material);
            await _appDbContext.SaveChangesAsync();

            return material;
        }

        public async Task<Material?> UpdateMaterialAsync(int id, string name)
        {
            var material =await _appDbContext.materials.FindAsync(id);
            if (material == null) return null;

            material.MaterialName = name;
            await _appDbContext.SaveChangesAsync();

            return material;
        }

        public async Task<Material?> DeleteMaterialAsync(int id)
        {
            var material = await _appDbContext.materials.FindAsync(id);
            if (material == null) return null;

            if (await _appDbContext.MaterialPurchase.Where(mp => mp.MaterialId == material.MaterialId).FirstOrDefaultAsync() == null)
            {
                _appDbContext.materials.Remove(material);
                await _appDbContext.SaveChangesAsync();
                return material;
            }

            material.Hide = true;
            await _appDbContext.SaveChangesAsync();
            return material;
        }
    }
}
