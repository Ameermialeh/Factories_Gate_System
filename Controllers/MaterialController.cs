using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static FactoriesGateSystem.DTOs.MaterialDTOs.AddMaterialDTO;
using static FactoriesGateSystem.DTOs.MaterialDTOs.UpdateMaterialDTO;

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
                };
                return Ok(materialdto);
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
            catch (Exception) { return StatusCode(500, "Internal server error."); }
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


        [HttpPut("UpdateMaterialName")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateMaterialName([FromBody] UpdateNameMaterialDTO dto)
        {
            if (dto.Id <= 0 || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Invalid material data.");
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var NameExists = await _materialRepo.ChickIfMaterialNameExistAsync(dto.Name, int.Parse(factoryId));
                if (!NameExists) return NotFound("Material already exists.");

                var material = await _materialRepo.UpdateMaterialNameAsync(dto,int.Parse(factoryId));
                if (material == null) return NotFound($"Material with id {dto.Id} not found.");
                return Ok(material);

            }
            catch(Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpPut("UpdateMaterialQuantity")]
        [ProducesResponseType(typeof(MaterialDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateMaterialQuantity([FromBody] UpdateQuantityMaterialDTO dto)
        {
            if (dto.Id <= 0 || dto.Quantity < 0)
                return BadRequest("Invalid material data.");
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var material = await _materialRepo.UpdateMaterialQuantityAsync(dto,int.Parse(factoryId));
                if (material == null) return NotFound($"Material with id {dto.Id} not found.");
                return Ok(material);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpDelete("{id}")]
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
