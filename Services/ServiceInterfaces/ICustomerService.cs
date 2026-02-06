using FactoriesGateSystem.Models.DTOs.CustomerDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<List<CustomerDTO>> GetCustomerNameAsync(string name);
        Task<CustomerDTO> CreateCustomerAsync(CustomerDTO dto);
        Task<CustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerDTO dto);
        Task DeleteCustomerAsync(int id);
    }
}
