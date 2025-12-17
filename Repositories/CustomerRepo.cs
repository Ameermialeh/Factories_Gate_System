using FactoriesGateSystem.DTOs.CustomerDTOs;
using FactoriesGateSystem.Models;

namespace FactoriesGateSystem.Repositories
{
    public class CustomerRepo
    {
        private readonly AppDbContext _appDbContext;

        public CustomerRepo(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public List<Customer> GetCustomers(Func<Customer, bool>? func = null)
        {
            var customers = _appDbContext.customer.ToList();
            if (func == null)
            {
                return customers;
            }
            customers = customers.Where(func).ToList();

            return customers;
        }

        public Customer? GetCustomerById(int id)
        {
            var customer = _appDbContext.customer.FirstOrDefault(c => c.CustomerId == id);
            return customer;
        }

        public CustomerDTO AddCustomer(CustomerDTO customerdto)
        {
           
            var customer = new Customer()
            {
                CustomerName = customerdto.Name,
                Address = customerdto.Address,
                PhoneNumber = customerdto.Phone,
                CurrentBalance = customerdto.CurrentBalance,
            };

            _appDbContext.customer.Add(customer);
            _appDbContext.SaveChanges();

            customerdto.ID = customer.CustomerId;
            return customerdto;
           
        }
        public UpdateCustomerDTO? UpdateCustomer(int id, UpdateCustomerDTO customer)
        {
            var existingCustomer = GetCustomerById(id);
            if (existingCustomer == null)
                return null;


            existingCustomer.CustomerName = customer.Name;
            existingCustomer.PhoneNumber = customer.Phone;
            existingCustomer.Address = customer.Address;
            existingCustomer.CurrentBalance = customer.CurrentBalance;

            _appDbContext.SaveChanges();

            return customer;
        }

        public Customer? DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer == null) { return null; }
           
            var orders = customer.Orders;
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    _appDbContext.orders.Remove(order);

                }
            }
            _appDbContext.customer.Remove(customer);
            _appDbContext.SaveChanges();
                
            return customer;
        }
    }
}
