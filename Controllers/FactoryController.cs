using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class FactoryController : Controller
    {
        private readonly IFactoryService _factoryService;
        public FactoryController(IFactoryService factoryService)
        {
            _factoryService = factoryService;
        }

        [HttpGet]

        [ProducesResponseType(typeof(List<FactoryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFactories([FromQuery] int? id , [FromQuery] int? userId)
        {
            var factories = await _factoryService.GetFactories(id, userId);
            return Ok(factories);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(List<FactoryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetFactoryById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Factory id.");

            var factory = await _factoryService.GetFactoryByIdAsync(id);
            return Ok(factory);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<FactoryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFactoryName(string name)
        {
            var factories = await _factoryService.GetFactoryNameAsync(name);
            return Ok(factories);
        }
    }
}
