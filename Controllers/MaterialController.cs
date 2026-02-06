using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.AddMaterialDTO;
using FactoriesGateSystem.Models.DTOs.MaterialDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MaterialDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaterials([FromQuery] string? name)
        {

            if (name == null)
            {
                var material = await _materialService.GetAllMaterials();
                return Ok(material);
            }
            var filtered = await _materialService.GetAllMaterialsWithNameAsync(name);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid material id.");

            var material = await _materialService.GetMaterialByIdAsync(id);
            return Ok(material);
        }


        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<MaterialDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaterialByName(string name)
        {
            var material = await _materialService.GetMaterialByNameAsync(name);
            return Ok(material);
        }


        [HttpPost("BuyNewMaterial")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddNewMaterial([FromBody] AddNewMaterialDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto.SupplierId <= 0 || dto.PricePerUnit <= 0 || dto.Quantity <= 0)
                return BadRequest("Material data can't be negative.");

            var material = await _materialService.AddNewMaterialAsync(dto);
            return Ok(material);
        }

        [HttpPost("BuyExistingMaterial")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddExistingMaterial([FromBody] AddExistingMaterialDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.MaterialId <= 0 || dto.SupplierId <=0 || dto.PricePerUnit <=0 || dto.Quantity <=0)
                return BadRequest("Material data can't be negative.");

            var material = await _materialService.AddExistingMaterialAsync(dto);
            return Ok(material);
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateMaterial(int id, [FromBody] UpdateMaterialDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid material id.");

            if (dto.Name == null && dto.Quantity == null)
                return BadRequest("At least one field (name or quantity) must be provided.");

            if (dto.Quantity < 0)
                return BadRequest("Quantity cannot be negative.");

            var material = await _materialService.UpdateMaterialAsync(id, dto);
            return Ok(material);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Material id.");

            await _materialService.DeleteMaterialAsync(id);
            return Ok("Material Deleted successfully");
        }
    }
}
