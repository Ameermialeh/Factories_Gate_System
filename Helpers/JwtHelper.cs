using System.Security.Claims;
using System.Text;

namespace FactoriesGateSystem.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration config;

        public JwtHelper (IConfiguration config)
        {
            this.config = config;
        }

        /*public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name , user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),

                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
    }
}
