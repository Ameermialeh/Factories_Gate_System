using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
    }
}
