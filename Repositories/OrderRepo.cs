using FactoriesGateSystem.DTOs.OrderDTOs;
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

        public bool ChickIfAllProductsNotHide(OrderWithProductsDTO dto)
        {
            var productIds = dto.Products.Select(p => p.ProductID).ToList();

            return _appDbContext.products.Where(p => productIds.Contains(p.ProductId)).All(p => !p.Hide);
        }

        public OrderWithProductsDTO CreateOrder(OrderWithProductsDTO dto)
        {
            Order order = new Order()
            {
                OrderName = dto.Name,
                OrderDate = dto.OrderDate,
                OrderDescription = dto.Description,
                CustomerId = dto.CustomerID
            };

            _appDbContext.orders.Add(order);
            _appDbContext.SaveChanges();

            foreach(var product in dto.Products)
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

            dto.ID = order.OrderId;
            return dto;
        }

        public OrderWithProductsDTO? UpdateOrder(OrderWithProductsDTO OrderWithProductsDto)
        {
            var order = GetOrderById(OrderWithProductsDto.ID);
            if (order == null) return null;
           
            order.OrderName = OrderWithProductsDto.Name;
            order.OrderDate = OrderWithProductsDto.OrderDate;
            order.OrderDescription = OrderWithProductsDto.Description;
            order.CustomerId = OrderWithProductsDto.CustomerID;
            _appDbContext.SaveChanges();

            var orderProducts = _appDbContext.orderProducts.Where(op => op.OrderId == 2).ToList();

            _appDbContext.orderProducts.RemoveRange(orderProducts);
            _appDbContext.SaveChanges();

            foreach (var product in OrderWithProductsDto.Products)
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

            return OrderWithProductsDto;
        }

        public Order? DeleteOrder(int id)
        {
            var order = GetOrderById(id);
            if (order == null) { return null; }

            _appDbContext.orders.Remove(order);
            _appDbContext.SaveChanges();
            return order;
        }
    }
}
