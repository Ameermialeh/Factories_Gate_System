using FactoriesGateSystem.DTOs.EmployeeDTOs;
using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : Controller
    {
        private readonly MaterialRepo _materialRepo;

        public MaterialController(MaterialRepo materialRepo)
        {
            _materialRepo = materialRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllMaterials()
        {
            try
            {
                var materialdto = await _materialRepo.GetMaterialAsync();
                if (materialdto == null || materialdto.Count == 0) { return BadRequest("There is no materials."); }

                return Ok(materialdto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid material id.");
            try
            {
                var material = await _materialRepo.GetMaterialByIdAsync(id);
                if (material == null)
                    return NotFound($"Material with id {id} not found.");

                var materialdto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.Name,
                    Unit = material.Unit,
                };
                return Ok(materialdto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
           
        }

        [HttpPost]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Unit))
                return BadRequest("Material name is required.");
            try
            {
                var material = await _materialRepo.CreateMaterialAsync(dto);

                var materialDto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.Name,
                    Unit = material.Unit,
                };
                return Ok(materialDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateMaterial(int id,[FromBody] CreateMaterialDTO dto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Unit))
                return BadRequest("Invalid material data.");
            try
            {
                var material = await _materialRepo.UpdateMaterialAsync(id, dto);
                if (material == null) return NotFound($"Material with id {id} not found.");

                var materialDto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.Name,
                    Unit = material.Unit,
                };
                return Ok(materialDto);
            }
            catch(Exception) 
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Material id.");
            try
            {
                var material =await _materialRepo.DeleteMaterialAsync(id);
                if (material == null) return NotFound($"Material with id {id} not found.");

                var materialDto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.Name,
                    Unit = material.Unit,
                };
                return Ok(materialDto);
            }
            catch (Exception) { 
                return StatusCode(500,"Internal server error.");
            }
        }
    }
}
