using FactoriesGateSystem.DTOs;
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

    }
}
