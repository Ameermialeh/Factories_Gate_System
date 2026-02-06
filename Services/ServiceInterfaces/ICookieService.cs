namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface ICookieService
    {
        void Set(string key, string value, CookieOptions options);
        string? Get(string key);
    }
}
