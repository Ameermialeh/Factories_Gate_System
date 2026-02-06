using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Set(string key, string value, CookieOptions options)
        {
            var context = _contextAccessor.HttpContext
                          ?? throw new InvalidOperationException("No active HTTP context.");

            context.Response.Cookies.Append(key, value, options);
        }
        public string? Get(string key)
        {
            var context = _contextAccessor.HttpContext
                ?? throw new InvalidOperationException("No active HTTP context.");

            context.Request.Cookies.TryGetValue(key, out var value);
            return value;
        }
    }
}
