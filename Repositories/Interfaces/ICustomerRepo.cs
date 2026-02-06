using FactoriesGateSystem.Models.DTOs.CustomerDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface ICustomerRepo
    {
        Task<List<CustomerDTO>> GetCustomersAsync(Expression<Func<Customer, bool>>? filter = null);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> AddCustomerAsync(CustomerDTO customerdto, int factoryId);
        Task<CustomerDTO?> UpdateCustomerAsync(int id, UpdateCustomerDTO dto);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
