namespace FactoriesGateSystem.Repositories
{
    public class SalaryRepo
    {
        private readonly AppDbContext _appDbContext;

        public SalaryRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
