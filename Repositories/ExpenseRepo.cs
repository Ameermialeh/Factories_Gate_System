namespace FactoriesGateSystem.Repositories
{
    public class ExpenseRepo
    {
        private readonly AppDbContext _appDbContext;

        public ExpenseRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }


    }
}
