using FactoriesGateSystem.Models.DTOs.ExpenseDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepo _expenseRepo;
        private readonly ICookieService _cookieService;
        public ExpenseService (IExpenseRepo expenseRepo, ICookieService cookieService)
        {
            _expenseRepo = expenseRepo;
            _cookieService = cookieService;
        }
        public async Task<List<ExpenseDTO>> GetAllExpenseAsync()
        {
            var Expenses = await _expenseRepo.GetAllExpenseAsync();
            return Expenses;
        }
        public async Task<List<ExpenseDTO>> GetExpenseWithDateAsync(DateTime date)
        {
            var expense = await _expenseRepo.GetAllExpenseAsync(e => e.Date >= date!.Date && e.Date < date.Date.AddDays(1));
            return expense;
        }
        public async Task<ExpenseDTO> GetExpenseByIdAsync(int id)
        {
            var expense = await _expenseRepo.GetExpenseByIdAsync(id)
            ?? throw new BadHttpRequestException("Expense Not Found", StatusCodes.Status404NotFound);

            var expenseDto = new ExpenseDTO
            {
                Id = expense.ExpenseId,
                Description = expense.Description,
                Date = expense.Date,
                Amount = expense.Amount,
            };
            return expenseDto;
        }

        public async Task<ExpenseDTO> AddExpenseAsync(AddExpenseDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var expense = await _expenseRepo.AddExpenseAsync(dto, int.Parse(factoryId));
            return expense;
        }
        public async Task<ExpenseDTO> UpdateExpenseAsync(int id, UpdateExpenseDTO dto)
        {
            var expense = await _expenseRepo.UpdateExpenseAsync(id, dto)
            ?? throw new BadHttpRequestException("Expense Not Found", StatusCodes.Status404NotFound);
            return expense;
        }
        public async Task DeleteExpenseAsync(int id)
        {
            var done = await _expenseRepo.DeleteExpenseAsync(id);
            if (!done) { throw new BadHttpRequestException("Expense Not Found", StatusCodes.Status404NotFound); }
        }
    }
}
