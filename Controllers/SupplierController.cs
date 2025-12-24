using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.DTOs.ProductDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : Controller
    {
       private readonly SupplierRepo _supplierRepo;
        public SupplierController(SupplierRepo supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliers()
        {
            try
            {
                var supplierdto = await _supplierRepo.GetSupplierAsync();
                if (supplierdto == null || supplierdto.Count == 0) { return NotFound("There is no suppliers."); }

                return Ok(supplierdto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }

        [HttpGet("/id")]
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
                    Name = supplier.SupplierName,
                    Address = supplier.Address,
                    Phone = supplier.SupplierPhone,
                    CurrentBalance = supplier.CurrentBalance
                };
                return Ok(supplierDto);
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
                var supplier = await _supplierRepo.AddSupplierAsync(supplierDto);
                return Ok(supplier);
            }
            catch (Exception) { return StatusCode(500, "Internal server error."); }
        }
    }
}
