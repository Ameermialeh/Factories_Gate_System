using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.VacationDTOs;
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

        [HttpGet]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllExpense()
        {
            try
            {
                var Expenses = await _expenseRepo.GetAllExpenseAsync();

                if (Expenses == null) { return NotFound("Expenses not Found!"); }
                return Ok(Expenses);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }
    }
}
