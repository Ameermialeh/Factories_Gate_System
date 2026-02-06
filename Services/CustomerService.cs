using FactoriesGateSystem.Models.DTOs.CustomerDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ICookieService _cookieService;
        public CustomerService(ICustomerRepo customerRepo, ICookieService cookieService)
        {
            _customerRepo = customerRepo;
            _cookieService = cookieService;
        }

        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var customerDto = await _customerRepo.GetCustomersAsync();
            return customerDto;
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepo.GetCustomerByIdAsync(id) 
                ?? throw new BadHttpRequestException("Customer Not Found", StatusCodes.Status404NotFound);

            var customerDto = new CustomerDTO()
            {
                ID = customer.CustomerId,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
            };
            return customerDto;
        }

        public async Task<List<CustomerDTO>> GetCustomerNameAsync(string name)
        {
            var customers = await _customerRepo.GetCustomersAsync(c => c.Name.Contains(name));
            return customers;
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CustomerDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var customer = await _customerRepo.AddCustomerAsync(dto, int.Parse(factoryId));
            return customer;
        }
        public async Task<CustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerDTO dto)
        {
            var customer = await _customerRepo.UpdateCustomerAsync(id, dto) 
                ?? throw new BadHttpRequestException("Customer Not Found", StatusCodes.Status404NotFound);

            return customer;
        }
        public async Task DeleteCustomerAsync(int id)
        {
            var done = await _customerRepo.DeleteCustomerAsync(id);
            if (!done)
                throw new BadHttpRequestException("Customer Not Found", StatusCodes.Status404NotFound);
        }
    }
}
