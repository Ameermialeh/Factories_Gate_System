using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    public class SalaryController : Controller
    {
        private readonly SalaryRepo _salaryRepo;

        public SalaryController(SalaryRepo salaryRepo)
        {
            _salaryRepo = salaryRepo;
        }
    }
}
