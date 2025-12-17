using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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
            try
            {
                var materials = _materialRepo.GetMaterial();
                if (materials == null || materials.Count == 0) { return BadRequest("There is no materials."); }
                var materialdto = materials.Select(m => new MaterialDTO()
                {
                    ID = m.MaterialId,
                    Name = m.MaterialName,
                });
                return Ok(materialdto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpGet("GetMaterialById/{id}")]
        public IActionResult GetMaterialById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid material id.");
            try
            {
                var material = _materialRepo.GetMaterialById(id);
                if (material == null)
                    return NotFound($"Material with id {id} not found.");

                var materialdto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.MaterialName,
                };
                return Ok(materialdto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
           
        }

        [HttpPost("CreateMaterial")]
        public IActionResult CreateMaterial([FromBody] CreateMaterialDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Material name is required.");
            try
            {
                var material = _materialRepo.CreateMaterial(dto.Name);

                var materialDto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.MaterialName,
                };
                return Ok(materialDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpPut("UpdateMaterialName/{id}")]
        public IActionResult UpdateMaterial(int id,[FromBody] CreateMaterialDTO dto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Invalid material data.");
            try
            {
                var material = _materialRepo.UpdateMaterial(id, dto.Name);
                if (material == null) return NotFound($"Material with id {id} not found.");

                var materialDto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.MaterialName,
                };
                return Ok(materialDto);
            }
            catch(Exception) 
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("DeleteMaterial/{id}")]
        public IActionResult DeleteMaterial(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Material id.");
            try
            {
                var material = _materialRepo.DeleteMaterial(id);
                if (material == null) return NotFound($"Material with id {id} not found.");

                var materialDto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.MaterialName,
                };
                return Ok(materialDto);
            }
            catch (Exception) { 
                return StatusCode(500,"Internal server error.");
            }
        }
    }
}
