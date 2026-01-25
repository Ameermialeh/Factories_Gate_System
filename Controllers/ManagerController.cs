using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ManagerController : Controller
    {
        private readonly ManagerRepo _managerRepo;

        public ManagerController(ManagerRepo managerRepo)
        {
            _managerRepo = managerRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ManagerDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllManagers([FromQuery] string? name)
        {
            try
            {
                if(name == null)
                {
                    var managers = await _managerRepo.GetManagersAsync();
                    return Ok(managers);
                }
                
                var filtered = await _managerRepo.GetManagersAsync(m => m.Name.Contains(name));
                return Ok(filtered);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ManagerDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetManagerById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Manager id.");
            try
            {
                var manager = await _managerRepo.GetManagersAsync(m => m.UserId == id);
                if (!manager.Any()) { return NotFound("No Manager Found!"); }
                return Ok(manager);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

    }
}
