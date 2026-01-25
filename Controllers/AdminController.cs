using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models.DTOs;
using FactoriesGateSystem.Models.DTOs.Admin;
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
        private readonly AuthRepo _authRepo;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtHelper _jwtHelper;
        public AdminController(AdminRepo adminRepo, PasswordHasher passwordHasher, AuthRepo authRepo, JwtHelper jwtHelper)
        {
            _adminRepo = adminRepo;
            _passwordHasher = passwordHasher;
            _authRepo = authRepo;
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<AdminDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var admins = await _adminRepo.GetAllAdminsAsync();
                return Ok(admins);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(AdminDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAdminName(string name)
        {
            try
            {
                var admins = await _adminRepo.GetAllAdminsAsync(a => a.Name.Contains(name));
                return Ok(admins);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterAdminDTO dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Name) ||
               string.IsNullOrWhiteSpace(dto.Email) ||
               string.IsNullOrWhiteSpace(dto.Password))
                    return BadRequest("Invalid Admin data.");
            
            try
            {
                var passwordHash = _passwordHasher.Hash(dto.Password);
                var user = await _authRepo.RegisterAdminAsync(dto, passwordHash);
                return Ok($"{user.Name} added Successfully");
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }


    }
}
