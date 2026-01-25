using FactoriesGateSystem.Models.DTOs.ExpenseDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.ExpenseDTOs.UpdateExpenseDTO;


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
        [ProducesResponseType(typeof(List<ExpenseDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExpense([FromQuery] DateTime? date)
        {
            try
            {
                if (date == null)
                {
                    var Expenses = await _expenseRepo.GetAllExpenseAsync();
                    return Ok(Expenses);
                }
                var filtered = await _expenseRepo.GetAllExpenseAsync(e =>e.Date >= date.Value.Date && e.Date < date.Value.Date.AddDays(1));
                return Ok(filtered);
            }
            catch { return StatusCode(500, "Internal server error"); }
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
            try
            {
                var expense = await _expenseRepo.GetExpenseByIdAsync(id);
                if (expense == null) { return NotFound($"No expense with id = {id}. "); }

                var expenseDto = new ExpenseDTO
                {
                    Id = expense.ExpenseId,
                    Description = expense.Description,
                    Date = expense.Date,
                    Amount = expense.Amount,
                };
                return Ok(expenseDto);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddExpense([FromBody] AddExpenseDTO dto)
        {
            if (dto.Amount <= 0 || String.IsNullOrWhiteSpace(dto.Description))
                return BadRequest("Invalid data.");

            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var expense = await _expenseRepo.AddExpenseAsync(dto,int.Parse(factoryId));
                return Ok(expense);
            }
            catch { return StatusCode(500, "Internal server error"); }
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

            try
            {
                var expense = await _expenseRepo.UpdateExpenseAsync(id, dto);
                if (expense == null) { return NotFound($"No Expense with id = {id}. "); }
                return Ok(expense);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid expense id.");
            try
            {
                var done = await _expenseRepo.DeleteExpenseAsync(id);
                if (!done) { return NotFound($"No expense with id = {id}. "); }

                return Ok($"Expense with {id} deleted Successfully");

            }
            catch { return StatusCode(500, "Internal server error"); }
        }
    }
}
