namespace FactoriesGateSystem.Repositories
{
    public class AdminRepo
    {
        private readonly AppDbContext _appDbContext;

        public AdminRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


    }
}
