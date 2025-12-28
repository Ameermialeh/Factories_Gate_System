using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.Admin;
using FactoriesGateSystem.DTOs.CustomerDTOs;
using FactoriesGateSystem.Helpers;
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

        [HttpGet("GetAllFactories")]
        [ProducesResponseType(typeof(FactoryDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllFactories()
        {
            try
            {
                var factories = await _adminRepo.GetAllFactoriesAsync();
                if (factories == null) { return NotFound("No Factories Found!"); }
                return Ok(factories);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
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
                if (managers == null) { return NotFound("No Managers Found!"); }
                return Ok(managers);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet("GetManagerById/{id}")]
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
                var manager = await _adminRepo.GetManagerByIdAsync(id);
                if (manager == null) { return NotFound("No Manager Found!"); }
                return Ok(manager);
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdminDTO), 200)]
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

        [HttpPost]
        [ProducesResponseType(typeof(RegisterAdminDTO), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterAdminDTO dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Invalid Admin data.");
            }
            try
            {
                var passwordHash = _passwordHasher.Hash(dto.Password);
                var user = await _authRepo.RegisterAdminAsync(dto, passwordHash);

                var accessToken = _jwtHelper.GenerateAccessToken(user);
                var refreshToken = _jwtHelper.GenerateRefreshToken();

                refreshToken.UserId = user.UserId;
                await _authRepo.SaveRefreshTokenAsync(refreshToken);

                return Ok(new
                {
                    accessToken,
                    refreshToken = refreshToken.Token
                });
            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(typeof(ChangePasswordDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || 
                string.IsNullOrWhiteSpace(dto.CurrentPassword) || 
                string.IsNullOrWhiteSpace(dto.NewPassword))
                return BadRequest("Invalid data.");
            try
            {
                var user =await _adminRepo.getAdminByEmailAsync(dto.Email);
                if(user == null) return NotFound("Admin not found");

                var isValid = _adminRepo.PasswordValid(user, dto);
                if (!isValid)
                    return BadRequest("Current Password is incorrect");

                await _adminRepo.UpdatePasswordAsync(user, dto);
                return Ok("Changed password successfully");

            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }
    }
}
