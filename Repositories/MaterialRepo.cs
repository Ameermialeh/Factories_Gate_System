using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.Models;

namespace FactoriesGateSystem.Repositories
{
    public class MaterialRepo
    {
        private readonly AppDbContext _appDbContext;

        public MaterialRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public List<Material> GetMaterial(Func<Material, bool>? func = null)
        {
            var material = _appDbContext.materials.Where(m => !m.Hide).ToList();
            if(func == null)
            {
                return material;
            }
            material = material.Where(func).ToList();
            return material;
        }

        public Material? GetMaterialById(int id)
        {
            var material = _appDbContext.materials.FirstOrDefault(m => m.MaterialId == id && !m.Hide);
            return material;
        }

        public Material CreateMaterial(string name)
        {
            var material = new Material()
            {
                MaterialName = name
            };

            _appDbContext.materials.Add(material);
            _appDbContext.SaveChanges();

            return material;
        }

        public Material? UpdateMaterial(int id, string name)
        {
            var material = _appDbContext.materials.Where(m=>m.MaterialId == id).FirstOrDefault();
            if (material == null) return null;

            material.MaterialName = name;
            _appDbContext.SaveChanges();

            return material;
        }

        public Material? DeleteMaterial(int id)
        {
            var material = GetMaterialById(id);
            if (material == null) return null;
            if (_appDbContext.MaterialPurchase.Where(mp => mp.MaterialId == material.MaterialId).FirstOrDefault() == null)
            {
                _appDbContext.materials.Remove(material);
                _appDbContext.SaveChanges();
                return material;
            }

            material.Hide = true;
            _appDbContext.SaveChanges();
            return material;
        }
    }
}
