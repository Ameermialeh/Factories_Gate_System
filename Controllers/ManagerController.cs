using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ManagerDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllManagers([FromQuery] string? name)
        {
            if(name == null)
            {
                var managers = await _managerService.GetAllManagersAsync();
                return Ok(managers);
            }
            var filtered = await _managerService.GetAllManagersWithNameAsync(name);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(List<ManagerDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetManagerById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Manager id.");

            var manager = await _managerService.GetManagerByIdAsync(id);
            return Ok(manager);
        }

    }
}
