namespace FactoriesGateSystem.DTOs
{
    public class AuthDTO
    {
        public class RegisterDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public string FactoryName { get; set; }

            public string Address { get; set; }
        }

        public class LoginDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RefreshTokenDTO
        {
            public string RefreshToken { get; set; }
        }
    }
}
