using FactoriesGateSystem.DTOs;
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

        public bool AddCustomer(CustomerDTO customerdto)
        {
            try
            {
                var customer = new Customer()
                {
                    CustomerId = customerdto.ID,
                    CustomerName = customerdto.Name,
                    Address = customerdto.Address,
                    PhoneNumber = customerdto.Phone,
                    CurrentBalance = customerdto.CurrentBalance,
                };
                 

                _appDbContext.customer.Add(customer);
                _appDbContext.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }
        public CustomerDTO? UpdateCustomer(CustomerDTO customer)
        {
            var existingCustomer = GetCustomerById(customer.ID);
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
            try
            {
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
            catch (Exception) {
                return null;            
            }


        }
    }
}
