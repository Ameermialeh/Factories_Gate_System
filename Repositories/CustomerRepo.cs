using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.CustomerDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class CustomerRepo
    {
        private readonly AppDbContext _appDbContext;

        public CustomerRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<List<CustomerDTO>> GetCustomersAsync(Expression<Func<Customer, bool>>? filter = null)
        {
            IQueryable<Customer> query = _appDbContext.customer;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.Select(c => new CustomerDTO
            {
                ID = c.CustomerId,
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone,
            }).ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _appDbContext.customer.FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO customerdto, int factoryId)
        {
           
            var customer = new Customer()
            {
                Name = customerdto.Name!,
                Address = customerdto.Address!,
                Phone = customerdto.Phone!,
                FactoryId = factoryId
            };

            await _appDbContext.customer.AddAsync(customer);
            await _appDbContext.SaveChangesAsync();

            customerdto.ID = customer.CustomerId;
            return customerdto;
           
        }
        public async Task<CustomerDTO?> UpdateCustomerAsync(int id, UpdateCustomerDTO dto)
        {
            var customer = await GetCustomerByIdAsync(id);
            if (customer == null) return null;

            if(dto.Name != null)
            {
                customer.Name = dto.Name;
            }

            if(dto.Address != null)
            {
                customer.Address = dto.Address;
            }

            if(dto.Phone != null)
            {
                customer.Phone = dto.Phone;
            }

            await _appDbContext.SaveChangesAsync();

            return new CustomerDTO
            {
                ID = customer.CustomerId,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
            };
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var existing = await GetCustomerByIdAsync(id);
            if (existing == null) return false;

            var orders = existing.Orders;
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    _appDbContext.orders.Remove(order);

                }
            }
            _appDbContext.customer.Remove(existing);
            await _appDbContext.SaveChangesAsync();
                
            return true;
        }
    }
}
