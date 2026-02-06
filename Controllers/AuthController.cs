using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.AuthDTO;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.CreateUserAsync(dto);

            return Ok(new { message = "Register successfully" });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accessToken = await _authService.LoginUserAsync(dto);

            return Ok(accessToken);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Logged out successfully" });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accessToken = await _authService.RefreshAsync(dto);

            return Ok(accessToken);
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ChangePasswordAsync(dto);

            return Ok(new { message = "Changed password successfully" });
        }

    }
}
