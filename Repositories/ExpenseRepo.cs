using FactoriesGateSystem.DTOs.ExpenseDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static FactoriesGateSystem.DTOs.ExpenseDTOs.UpdateExpenseDTO;

namespace FactoriesGateSystem.Repositories
{
    public class ExpenseRepo
    {
        private readonly AppDbContext _appDbContext;

        public ExpenseRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<List<ExpenseDTO>?> GetAllExpenseAsync(Expression<Func<Expense, bool>>? filter = null)
        {
            IQueryable<Expense> query = _appDbContext.expenses;
            if (filter != null)
                query = query.Where(filter);
            return await query.Select(e => new ExpenseDTO
            {
                Id = e.ExpenseId,
                Description = e.Description,
                Date = e.Date,
                Amount = e.Amount,
            }).ToListAsync();
        }


        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            return await _appDbContext.expenses.Where(v => v.ExpenseId == id).FirstOrDefaultAsync();
        }

        public async Task<ExpenseDTO> AddExpenseAsync(AddExpenseDTO dto, int factoryId)
        {
            var expense = new Expense
            {
                Description = dto.Description!,
                Amount = dto.Amount,
                Date = dto.Date,
                FactoryId = factoryId,
            };

            await _appDbContext.expenses.AddAsync(expense);
            await _appDbContext.SaveChangesAsync();
            return new ExpenseDTO
            {
                Id = expense.ExpenseId,
                Description = dto.Description,
                Date = dto.Date,
                Amount = dto.Amount,
            };
        }

        public async Task<ExpenseDTO?> UpdateExpenseAmountAsync(UpdateExpenseAmount dto)
        {
            var expense = await GetExpenseByIdAsync(dto.id);
            if (expense == null) { return null; }

            expense.Amount = dto.newAmount;

            await _appDbContext.SaveChangesAsync();

            return new ExpenseDTO
            {
                Id = dto.id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
            };
        }
        public async Task<ExpenseDTO?> UpdateExpenseDescriptionAsync(UpdateExpenseDescription dto)
        {
            var expense = await GetExpenseByIdAsync(dto.id);
            if (expense == null) { return null; }

            expense.Description = dto.newDescription!;

            await _appDbContext.SaveChangesAsync();

            return new ExpenseDTO
            {
                Id = dto.id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
            };
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await GetExpenseByIdAsync(id);
            if (expense == null) { return false; }

            _appDbContext.expenses.Remove(expense);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
