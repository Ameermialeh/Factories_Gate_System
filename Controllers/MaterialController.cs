using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.AddMaterialDTO;
using static FactoriesGateSystem.Models.DTOs.MaterialDTOs.UpdateMaterialDTO;
using FactoriesGateSystem.Models.DTOs.MaterialDTOs;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class MaterialController : Controller
    {
        private readonly MaterialRepo _materialRepo;
        private readonly SupplierRepo _supplierRepo;
        public MaterialController(MaterialRepo materialRepo,SupplierRepo supplierRepo)
        {
            _materialRepo = materialRepo;
            _supplierRepo = supplierRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaterials([FromQuery] string? name)
        {
            try
            {
                if (name == null)
                {
                    var materialdto = await _materialRepo.GetMaterialAsync();
                    return Ok(materialdto);
                }
                var filtered = await _materialRepo.GetMaterialAsync(m => m.Name.Contains(name));
                return Ok(filtered);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
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
            try
            {
                var material = await _materialRepo.GetMaterialByIdAsync(id);
                if (material == null)
                    return NotFound($"Material with id {id} not found.");

                var materialdto = new MaterialDTO()
                {
                    ID = material.MaterialId,
                    Name = material.Name,
                    Quantity = material.Inventory!.Quantity
                };
                return Ok(materialdto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
           
        }


        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<MaterialDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaterialByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Invalid material name.");
            try
            {
                var material = await _materialRepo.GetMaterialAsync(m => m.Name.Contains(name));
                return Ok(material);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }


        [HttpPost("BuyNewMaterial")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddNewMaterial([FromBody] AddNewMaterialDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || 
                dto.SupplierId <= 0 || 
                dto.PricePerUnit <=0 || 
                dto.Quantity < 0)
                return BadRequest("Material data is invalid.");
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var supplierExists = await _supplierRepo.ChickIfSupplierExistAsync(dto.SupplierId, int.Parse(factoryId));
                if (!supplierExists) return NotFound("Supplier not found.");

                var NameExists = await _materialRepo.ChickIfMaterialNameExistAsync(dto.Name, int.Parse(factoryId));
                if (NameExists) return NotFound("Material already exists.");

                var materialDto = await _materialRepo.AddNewMaterialAsync(dto, int.Parse(factoryId));
                return Ok(materialDto);
            }
            catch (Exception ex) { return StatusCode(500, ex); }
        }

        [HttpPost("BuyExistingMaterial")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddExistingMaterial([FromBody] AddExistingMaterialDTO dto)
        {
            if (dto.MaterialId <= 0 || dto.SupplierId <=0 || dto.PricePerUnit <=0 || dto.Quantity <=0)
                return BadRequest("Material data is invalid.");
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var materialExists = await _materialRepo.ChickIfMaterialExistAsync(dto.MaterialId, int.Parse(factoryId));
                if (!materialExists) return NotFound("Material not found.");


                var supplierExists = await _supplierRepo.ChickIfSupplierExistAsync(dto.SupplierId,int.Parse(factoryId));
                if(!supplierExists) return NotFound("Supplier not found.");


                var material = await _materialRepo.AddExistingMaterialAsync(dto);
                return Ok(material);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
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

            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                if (dto.Name != null)
                {
                    var nameExists = await _materialRepo
                        .ChickIfMaterialNameExistAsync(dto.Name, int.Parse(factoryId));

                    if (!nameExists)
                        return NotFound("Material name already exists.");
                }
                var material = await _materialRepo.UpdateMaterialAsync(id, dto, int.Parse(factoryId));
                if (material == null)
                    return NotFound($"Material with id {id} not found.");

                return Ok(material);
            }
            catch(Exception) { return StatusCode(500, "Internal server error."); }
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
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var IsZero = await _materialRepo.chickIfMaterialQuantityZeroAsync(id, int.Parse(factoryId));
                if(!IsZero) return BadRequest("Cannot delete material because the quantity is not zero.");

                var material =await _materialRepo.DeleteMaterialAsync(id, int.Parse(factoryId));
                if (!material) return NotFound($"Material with id {id} not found.");

                return Ok("Material Deleted successfully");
            }
            catch (Exception) { 
                return StatusCode(500,"Internal server error.");
            }
        }
    }
}
