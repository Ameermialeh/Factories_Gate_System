using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.AuthDTO;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthRepo _authRepo;
        private readonly FactoryRepo _factoryRepo;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher _passwordHasher;

        public AuthController(AuthRepo authRepo, JwtHelper jwtHelper, PasswordHasher passwordHasher, FactoryRepo factoryRepo)
        {
            _authRepo = authRepo;

            _jwtHelper = jwtHelper;

            _passwordHasher = passwordHasher;
            _factoryRepo = factoryRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.Name)||
                string.IsNullOrWhiteSpace(dto.FactoryName) ||
                string.IsNullOrWhiteSpace(dto.Address))
                    return BadRequest("Invalid data.");
            try
            {
                var passwordHash = _passwordHasher.Hash(dto.Password);

                var user = await _authRepo.RegisterAsync(dto, passwordHash);

                return Ok("Register successfully, Try to login");
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _authRepo.LoginAsync(dto);
            if (user == null)
                return NotFound("User Not Found");

            if (!_passwordHasher.Verify(dto.Password!, user.PasswordHash))
                return BadRequest("Password is incorrect");

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            refreshToken.UserId = user.UserId;
            await _authRepo.SaveRefreshTokenAsync(refreshToken);


            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),
                HttpOnly = true,
                Secure = true
            };

            Response.Cookies.Append("UserId", $"{user.UserId}", options);
            if(user.Role == "manager")
            {
                var factoryId = await _factoryRepo.GetFactoryId(user.UserId);
                Response.Cookies.Append("FactoryId", $"{factoryId}", options);
            }

            return Ok(new
            {
                accessToken,
                refreshToken = refreshToken.Token
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {

            var userId = Request.Cookies["UserId"];
            if (userId == null)
                return Unauthorized();

            var done =await _authRepo.LogoutAsync(int.Parse(userId));
            if (!done) { return BadRequest("Something went wrong"); }

            return Ok(new { message = "Logged out successfully" });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDTO dto)
        {

            var user = await _authRepo.GetUserByRefreshTokenAsync(dto.RefreshToken!);
            if (user == null)
                return Unauthorized();

            await _authRepo.RevokeRefreshTokenAsync(dto.RefreshToken!);

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();
            newRefreshToken.UserId = user.UserId;

            await _authRepo.SaveRefreshTokenAsync(newRefreshToken);

            return Ok(new
            {
                accessToken,
                refreshToken = newRefreshToken.Token
            });
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(200)]
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
                var user = await _authRepo.getAUserByEmailAsync(dto.Email);
                if (user == null) return NotFound("User not found");

                var isValid = _authRepo.PasswordValid(user, dto);
                if (!isValid)
                    return BadRequest("Current Password is incorrect");

                await _authRepo.UpdatePasswordAsync(user, dto);
                return Ok("Changed password successfully");

            }
            catch (Exception) { return StatusCode(500, "Internal Server Error"); }
        }

    }
}
