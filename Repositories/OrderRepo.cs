using FactoriesGateSystem.Models;

namespace FactoriesGateSystem.Repositories
{
    public class OrderRepo
    {
        private readonly AppDbContext _appDbContext;

        public OrderRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Order> GetOrders(Func<Order,bool> func = null)
        {
            var orders = _appDbContext.orders.ToList();
            if(func == null)
            {
                return orders;
            }
            orders = orders.Where(func).ToList();
            return orders;
        }


        public Order? GetOrderById(int id) {
            var order = _appDbContext.orders.FirstOrDefault(o => o.OrderId == id);
            return order;
        }

        public bool AddOrder(Order order)
        {
            try
            {
                _appDbContext.orders.Add(order);
                _appDbContext.SaveChanges();
                return true;
            }
            catch { 
                return false;
            }
        }
    }
}
