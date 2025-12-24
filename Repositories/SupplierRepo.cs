namespace FactoriesGateSystem.Repositories
{
    public class SupplierRepo
    {
        private readonly AppDbContext _appDbContext;

        public SupplierRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
