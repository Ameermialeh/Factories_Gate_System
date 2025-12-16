using FactoriesGateSystem.DTOs;
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
            var material = _appDbContext.materials.ToList();
            if(func == null)
            {
                return material;
            }
            material = material.Where(func).ToList();
            return material;
        }

        public Material? GetMaterialById(int id)
        {
            var material = _appDbContext.materials.FirstOrDefault(m => m.MaterialId == id);
            return material;
        }

        public Material? CreateMaterial(string name)
        {
            try
            {
                var material = new Material()
                {
                    MaterialName = name
                };
                _appDbContext.materials.Add(material);
                _appDbContext.SaveChanges();
                return material;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
