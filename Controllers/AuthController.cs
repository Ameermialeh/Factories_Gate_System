using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.DTOs.AuthDTO;

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
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
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

            var factoryId = await _factoryRepo.GetFactoryId(user.UserId);
            Response.Cookies.Append("FactoryId", $"{factoryId}", options);

            return Ok(new
            {
                accessToken,
                refreshToken = refreshToken.Token
            });
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

    }
}
