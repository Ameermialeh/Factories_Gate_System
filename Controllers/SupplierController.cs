using FactoriesGateSystem.Models.DTOs.SupplierDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class SupplierController : Controller
    {
       private readonly SupplierRepo _supplierRepo;
        public SupplierController(SupplierRepo supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SupplierDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliers([FromQuery] string? name)
        {
            try
            {
                if (name == null)
                {
                    var supplierdto = await _supplierRepo.GetSupplierAsync();
                    return Ok(supplierdto);
                }
                var filtered = await _supplierRepo.GetSupplierAsync(s => s.Name.Contains(name));
                return Ok(filtered);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSupplierByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid supplier id.");
            try
            {
                var supplier = await _supplierRepo.GetSupplierByIdAsync(id);
                if (supplier == null)
                    return NotFound($"Supplier with id {id} not found.");

                var supplierDto = new SupplierDTO()
                {
                    Id = id,
                    Name = supplier.Name,
                    Address = supplier.Address,
                    Phone = supplier.Phone,
                };
                return Ok(supplierDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<SupplierDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliersByName(string name)
        {
            try
            {
                var supplier = await _supplierRepo.GetSupplierAsync(s => s.Name.Contains(name));
                return Ok(supplier);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierDTO supplierDto)
        {
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var supplier = await _supplierRepo.AddSupplierAsync(supplierDto, int.Parse(factoryId));
                return Ok(supplier);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDTO supplierDto)
        {
            if (id <= 0)
                return BadRequest("Invalid supplier id.");

            if(supplierDto.Name == null && supplierDto.Address == null && supplierDto.Phone == null)
                return BadRequest("At least one field (Name or Address or Phone) must be provided.");

            try
            {
                var supplier = await _supplierRepo.UpdateSupplierAsync(id, supplierDto);
                if (supplier == null) { return NotFound($"No Supplier with id: {id}."); }
                return Ok(supplier);
            }
            catch (Exception)
            { return StatusCode(500, "Internal server error"); }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteSupplierDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid supplier id.");
            try
            {
                var supplier = await _supplierRepo.DeleteSupplierAsync(id);
                if (supplier == null) { return NotFound($"No Supplier with id: {id}."); }

                var supplierDto = new DeleteSupplierDTO()
                {
                    Id = id,
                    Name = supplier.Name,
                };
                return Ok(supplierDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
        }
    }
} 
