using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class ExpenseController : Controller
    {
        private readonly ExpenseRepo _expenseRepo;

        public ExpenseController(ExpenseRepo expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }


    }
}
