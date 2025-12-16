using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class MaterialController : Controller
    {
        private readonly MaterialRepo _materialRepo;

        public MaterialController(MaterialRepo materialRepo)
        {
            _materialRepo = materialRepo;
        }

        [HttpGet("GetAllMaterials")]
        public IActionResult GetAllMaterials()
        {
            var materials = _materialRepo.GetMaterial();
            if(materials == null || materials.Count == 0)
            {
                return BadRequest("There is no materials.");
            }
            var materialdto = materials.Select(m => new MaterialDTO()
            {
                ID = m.MaterialId,
                Name = m.MaterialName,
            });
            return Ok(materialdto);
        }

        [HttpGet("GetMaterialById/{id}")]
        public IActionResult GetMaterialById(int id)
        {
            var material = _materialRepo.GetMaterialById(id);
            if(material == null)
            {
                return BadRequest($"No material with id = {id}. Try again");
            }
            var materialdto = new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.MaterialName,
            };
            return Ok(materialdto);
        }

        [HttpPost("CreateMaterial")]
        public IActionResult CreateMaterial(string name)
        {
            var material = _materialRepo.CreateMaterial(name);
            if(material == null)
            {
                return BadRequest("Somthing went wrong!");
            }
            var materialdto = new MaterialDTO()
            {
                ID = material.MaterialId,
                Name = material.MaterialName,
            };
            return Ok(materialdto);
        }
    }
}
