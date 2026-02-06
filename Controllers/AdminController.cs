using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;

        public AdminController(IAuthService authService,IAdminService adminService)
        {
             _adminService = adminService;
            _authService = authService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AdminDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(AdminDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAdminName([FromQuery, Required] string name)
        {
            var admins = await _adminService.GetAdminNameAsync(name);
            return Ok(admins);
             
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterAdminDTO dto)
        {
            if(!ModelState.IsValid)
                    return BadRequest(ModelState);

            await _authService.CreateAdminAsync(dto);
            return Ok("Admin added successfully.");
        }
    }
}
