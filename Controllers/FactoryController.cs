using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class FactoryController : Controller
    {
        private readonly FactoryRepo _factoryRepo;

        public FactoryController(FactoryRepo factoryRepo)
        {
            _factoryRepo = factoryRepo;
        }

        [HttpGet]

        [ProducesResponseType(typeof(List<FactoryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFactories([FromQuery] int? id , [FromQuery] int? userId)
        {
            try
            {
                var factories = await _factoryRepo.GetFactoryAsync(f => (id == null || f.FactoryId == id.Value) && (userId == null || f.UserId == userId.Value));

                if (!factories.Any())
                    return NotFound("No factories found.");

                return Ok(factories);

            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FactoryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetFactoryById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Factory id.");
            try
            {
                var factory = await _factoryRepo.GetFactoryAsync(f => f.FactoryId == id);
                if (factory == null) { return NotFound($"No Factory with id = {id}."); }
                return Ok(factory);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<FactoryDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFactoryName(string name)
        {
            try
            {
                var factories = await _factoryRepo.GetFactoryAsync(f => f.Name.Contains(name));

                if (!factories.Any())
                    return NotFound("No factories found.");

                return Ok(factories);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }
    }
}
