using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.CustomerDTOs;
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
        [ProducesResponseType(typeof(FactoryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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

        [HttpGet("GetFactoryById/{id}")]
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
                var factory = await _adminRepo.GetFactoryByIdAsync(id);
                if (factory == null) { return NotFound($"No Factory with id = {id}."); }
                return Ok(factory);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet("GetAllManagers")]
        [ProducesResponseType(typeof(ManagerDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllManagers()
        {
            try
            {
                var managers = await _adminRepo.GetAllManagersAsync();
                if(managers == null) { return NotFound("No Managers Found!");  }
                return Ok(managers);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }


    }


}
