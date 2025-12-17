using FactoriesGateSystem.DTOs;

namespace FactoriesGateSystem.Repositories
{
    public class EmployeeRepo
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

       // public EmployeeDTO CreateEmployee(EmployeeDTO employeeDTO)
        //{


        //}
    }
}
