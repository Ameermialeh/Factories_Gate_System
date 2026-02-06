using FactoriesGateSystem.Models.DTOs.ExpenseDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ExpenseDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExpense([FromQuery] DateTime? date)
        {

            if (date == null)
            {
                var Expenses = await _expenseService.GetAllExpenseAsync();
                return Ok(Expenses);
            }
            var filtered = await _expenseService.GetExpenseWithDateAsync(date.Value);
            return Ok(filtered);

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid expense id.");

            var expense = await _expenseService.GetExpenseByIdAsync(id);
            return Ok(expense);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddExpense([FromBody] AddExpenseDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var expense = await _expenseService.AddExpenseAsync(dto);
            return Ok(expense);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid Expense id.");

            if(dto.Amount == null && dto.Description == null)
                return BadRequest("At least one field (Amount or Description) must be provided.");

            if(dto.Amount < 0)
                return BadRequest("Amount cannot be negative.");

            var expense = await _expenseService.UpdateExpenseAsync(id, dto);
            return Ok(expense);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid expense id.");

            await _expenseService.DeleteExpenseAsync(id);

            return Ok($"Expense with {id} deleted Successfully");
        }
    }
}
