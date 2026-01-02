namespace FactoriesGateSystem.Repositories
{
    public class VacationRepo
    {
        private readonly AppDbContext _appDbContext;

        public VacationRepo (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
