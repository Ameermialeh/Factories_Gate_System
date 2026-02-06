using FactoriesGateSystem.Models.DTOs.SupplierDTOs;
using FactoriesGateSystem.Repositories;
using FactoriesGateSystem.Services.ServiceInterfaces;
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
        private readonly ISupplierService _supplierService;
        public SupplierController(SupplierRepo supplierRepo,ISupplierService supplierService)
        {
            _supplierRepo = supplierRepo;
            _supplierService = supplierService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SupplierDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliers([FromQuery] string? name)
        {
            if (name == null)
            {
                var supplier = await _supplierService.GetAllSuppliers();
                return Ok(supplier);
            }
            var filtered = await _supplierService.GetAllSuppliersWithNameAsync(name);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid supplier id.");

            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            return Ok(supplier);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<SupplierDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliersByName(string name)
        {
            var supplier = await _supplierService.GetSuppliersByNameAsync(name);
            return Ok(supplier);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSupplier([FromBody] CreateSupplierDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _supplierService.AddSupplierAsync(dto);
            return Ok(supplier);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SupplierDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid supplier id.");

            if(dto.Name == null && dto.Address == null && dto.Phone == null)
                return BadRequest("At least one field (Name or Address or Phone) must be provided.");

            var supplier = await _supplierService.UpdateSupplierAsync(id, dto);
            return Ok(supplier);
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

            var supplier = await _supplierService.DeleteSupplierAsync(id);
            return Ok(supplier);
        }
    }
} 
