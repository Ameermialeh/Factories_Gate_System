using FactoriesGateSystem.DTOs.ExpenseDTOs;
using FactoriesGateSystem.DTOs.VacationDTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.DTOs.ExpenseDTOs.UpdateExpenseDTO;
using static FactoriesGateSystem.DTOs.VacationDTOs.UpdateVacationDTO;

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

        [HttpGet("{id}")]
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
        public async Task<IActionResult> AddExpense(AddExpenseDTO dto)
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

        [HttpPut("UpdateExpenseAmount")]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateExpenseAmount(UpdateExpenseAmount dto)
        {
            if (dto.id <= 0 || dto.newAmount < 0)
                return BadRequest("Invalid Expense data.");
            try
            {
                var expense = await _expenseRepo.UpdateExpenseAmountAsync(dto);
                if (expense == null) { return NotFound($"No Expense with id = {dto.id}. "); }
                return Ok(expense);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }

        [HttpPut("UpdateExpenseDescription")]
        [ProducesResponseType(typeof(ExpenseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateExpenseDescription(UpdateExpenseDescription dto)
        {
            if (dto.id <= 0 || string.IsNullOrWhiteSpace(dto.newDescription))
                return BadRequest("Invalid Expense data.");
            try
            {
                var expense = await _expenseRepo.UpdateExpenseDescriptionAsync(dto);
                if (expense == null) { return NotFound($"No Expense with id = {dto.id}. "); }
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
