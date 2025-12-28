using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AdminRepo _adminRepo;

        public AdminController(AdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        [HttpGet("GetAllFactories")]
        public async Task<IActionResult> GetAllFactories()
        {
            try
            {
                var factories = await _adminRepo.GetAllFactoriesAsync();
                if(factories == null) { return NotFound("No Factories Found!"); }
                return Ok(factories);
            }
            catch (Exception){ return StatusCode(500, "Internal Server Error"); }
        }
    }
}
