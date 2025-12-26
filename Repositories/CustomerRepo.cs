using FactoriesGateSystem.DTOs.CustomerDTOs;
using FactoriesGateSystem.Models;
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

        public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO customerdto)
        {
           
            var customer = new Customer()
            {
                Name = customerdto.Name!,
                Address = customerdto.Address!,
                Phone = customerdto.Phone!,
            };

            await _appDbContext.customer.AddAsync(customer);
            await _appDbContext.SaveChangesAsync();

            customerdto.ID = customer.CustomerId;
            return customerdto;
           
        }
        public async Task<UpdateCustomerDTO?> UpdateCustomerAsync(int id, UpdateCustomerDTO customer)
        {
            var existing = await _appDbContext.customer.FindAsync(id);
            if (existing == null)
                return null;


            existing.Name = customer.Name!;
            existing.Phone = customer.Phone!;
            existing.Address = customer.Address!;

            await _appDbContext.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer?> DeleteCustomerAsync(int id)
        {
            var existing = await _appDbContext.customer.FindAsync(id);
            if (existing == null)
                return null;

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
                
            return existing;
        }
    }
}
