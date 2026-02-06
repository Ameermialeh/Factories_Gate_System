using FactoriesGateSystem.Models.DTOs.ExpenseDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IExpenseRepo
    {
        Task<List<ExpenseDTO>> GetAllExpenseAsync(Expression<Func<Expense, bool>>? filter = null);
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task<ExpenseDTO> AddExpenseAsync(AddExpenseDTO dto, int factoryId);
        Task<ExpenseDTO?> UpdateExpenseAsync(int id, UpdateExpenseDTO dto);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
