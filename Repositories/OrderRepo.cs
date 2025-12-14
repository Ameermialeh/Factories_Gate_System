using FactoriesGateSystem.DTOs;
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

        public List<OrderProductsDTO> GetProductsForOrder(int orderID)
        {
            var products =_appDbContext.orderProducts.Where(op=> op.OrderId == orderID).Select(op => new OrderProductsDTO
            {
                ProductID = op.ProductId,
                ProductQuantity = op.Quantity
            }).ToList();
            
            return products;
        }

        public bool CreateOrder(OrderWithProductsDTO OrderWithProductsDto)
        {
            try
            {
                Order order = new Order()
                {
                    OrderName = OrderWithProductsDto.Name,
                    OrderDate = OrderWithProductsDto.OrderDate,
                    OrderDescription = OrderWithProductsDto.Description,
                    CustomerId = OrderWithProductsDto.CustomerID
                };

                _appDbContext.orders.Add(order);
                _appDbContext.SaveChanges();

                 

                foreach(var product in OrderWithProductsDto.Products)
                {
                    var orderProduct = new OrderProduct()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.ProductID,
                        Quantity = product.ProductQuantity
                    };

                    _appDbContext.orderProducts.Add(orderProduct);
                }
                _appDbContext.SaveChanges();
                return true;
            }
            catch { 
                return false;
            }
        }

        public OrderDTO? UpdateOrder(OrderDTO orderdto)
        {
            var newOrder = GetOrderById(orderdto.ID);
            if (newOrder == null) { return null; }

            try
            {
                newOrder.OrderId = orderdto.ID;
                newOrder.OrderName = orderdto.Name;
                newOrder.OrderDate = orderdto.OrderDate;
                newOrder.OrderDescription = orderdto.Description;
                newOrder.CustomerId = orderdto.CustomerID;
                _appDbContext.SaveChanges();
                return orderdto;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Order? DeleteOrder(int id)
        {
            var order = GetOrderById(id);
            if (order == null) { return null; }
            try
            {
                _appDbContext.orders.Remove(order);
                _appDbContext.SaveChanges();
                return order;
            }
            catch (Exception)
            {
                return null;
            }


        }
    }
}
