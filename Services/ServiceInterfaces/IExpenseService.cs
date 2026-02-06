using FactoriesGateSystem.Models.DTOs.ExpenseDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IExpenseService
    {
        Task<List<ExpenseDTO>> GetAllExpenseAsync();
        Task<List<ExpenseDTO>> GetExpenseWithDateAsync(DateTime date);
        Task<ExpenseDTO> GetExpenseByIdAsync(int id);
        Task<ExpenseDTO> AddExpenseAsync(AddExpenseDTO dto);
        Task<ExpenseDTO> UpdateExpenseAsync(int id, UpdateExpenseDTO dto);
        Task DeleteExpenseAsync(int id);
    }
}
