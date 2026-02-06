using FactoriesGateSystem.Helpers;
using FactoriesGateSystem.Models.DTOs.Admin;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;
using static FactoriesGateSystem.Models.DTOs.AuthDTO;

namespace FactoriesGateSystem.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtHelper _jwtHelper;
        private readonly ICookieService _cookieService;
        private readonly IFactoryRepo _factoryRepo;
        public AuthService(IAuthRepo authRepo, IPasswordHasher passwordHasher, IJwtHelper jwtHelper, ICookieService cookieService, IFactoryRepo factoryRepo)
        {
            _authRepo = authRepo;
            _passwordHasher = passwordHasher;
            _jwtHelper = jwtHelper;
            _cookieService = cookieService;
            _factoryRepo = factoryRepo;
        }

        public async Task CreateAdminAsync(RegisterAdminDTO dto)
        {
            if (await _authRepo.EmailExistsAsync(dto.Email))
                throw new BadHttpRequestException("Email already exists", StatusCodes.Status409Conflict);

            var passwordHash = _passwordHasher.Hash(dto.Password);

            await _authRepo.RegisterAdminAsync(dto, passwordHash);
        }

        public async Task CreateUserAsync(RegisterDTO dto)
        {
            if (await _authRepo.EmailExistsAsync(dto.Email))
                throw new BadHttpRequestException("Email already exists", StatusCodes.Status409Conflict);

            if (await _authRepo.FactoryNameExistsAsync(dto.Email))
                throw new BadHttpRequestException("Factory name already exists", StatusCodes.Status409Conflict);

            var passwordHash = _passwordHasher.Hash(dto.Password);
            await _authRepo.RegisterAsync(dto, passwordHash);
        }
        public async Task<string> LoginUserAsync(LoginDTO dto)
        {
            var user = await _authRepo.LoginAsync(dto);

            if (user == null)
                throw new BadHttpRequestException("User Not Found", StatusCodes.Status404NotFound);

            if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
                throw new BadHttpRequestException("Password is incorrect", StatusCodes.Status400BadRequest);

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

            _cookieService.Set(
            key: "UserId",
            value: $"{user.UserId}",
            options: options);

            if (user.Role == "manager")
            {
                var factoryId = await _factoryRepo.GetFactoryId(user.UserId);
                _cookieService.Set(
                    key: "FactoryId",
                    value: $"{factoryId}",
                    options: options);
            }
            return accessToken;
        }

        public async Task LogoutAsync()
        {
            var userId = _cookieService.Get("UserId") 
                ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var done = await _authRepo.LogoutAsync(int.Parse(userId));
            if (!done) { throw new BadHttpRequestException("No Refresh Token found", StatusCodes.Status400BadRequest); }
        }

        public async Task<string> RefreshAsync(RefreshTokenDTO dto)
        {
            var user = await _authRepo.GetUserByRefreshTokenAsync(dto.RefreshToken)
                ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);


            await _authRepo.RevokeRefreshTokenAsync(dto.RefreshToken);

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();
            newRefreshToken.UserId = user.UserId;

            await _authRepo.SaveRefreshTokenAsync(newRefreshToken);

            return accessToken;
        }
        public async Task ChangePasswordAsync(ChangePasswordDTO dto)
        {
            var user = await _authRepo.getAUserByEmailAsync(dto.Email)
                ?? throw new BadHttpRequestException("User Not Found", StatusCodes.Status404NotFound);

            var isValid = _authRepo.PasswordValid(user, dto);
            if (!isValid)
                throw new BadHttpRequestException("Current Password is incorrect", StatusCodes.Status400BadRequest);

            await _authRepo.UpdatePasswordAsync(user, dto);
        }
    }
}
