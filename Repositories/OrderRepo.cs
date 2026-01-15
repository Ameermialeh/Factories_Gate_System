using FactoriesGateSystem.DTOs.OrderDTOs;
using FactoriesGateSystem.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace FactoriesGateSystem.Repositories
{
    public class OrderRepo
    {
        private readonly AppDbContext _appDbContext;

        public OrderRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(Expression<Func<Order,bool>>? filter = null)
        {
            IQueryable<Order> query = _appDbContext.orders;
            if (filter != null)
                query = query.Where(filter);
            return await query.Select(o => new OrderDTO()
            {
                ID = o.OrderId,
                Name = o.Name,
                OrderDate = o.OrderDate,
                CustomerID = o.CustomerId,
                FactoryId = o.FactoryId,
            }).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id) {
            return await _appDbContext.orders.FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<List<OrderItemDTO>> GetProductsForOrderAsync(int orderID)
        {
            var products =await _appDbContext.OrderItem.Where(op=> op.OrderId == orderID).Select(op => new OrderItemDTO
            {
                ProductID = op.ProductId,
                ProductQuantity = op.Quantity
            }).ToListAsync();
            
            return products;
        }

        public async Task<OrderWithProductsDTO?> CreateOrderAsync(OrderWithProductsDTO dto, int factoryId)
        {
            var total = 0;
            Order order = new Order()
            {
                Name = dto.Name!,
                OrderDate = dto.OrderDate,
                CustomerId = dto.CustomerID,
                FactoryId = factoryId
            };

            await _appDbContext.orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            
            foreach(var product in dto.Products!)
            {
                var p = await _appDbContext.products.Where(P => P.ProductId == product.ProductID).FirstOrDefaultAsync();
                if(p == null) { return null; }

                total += (p.Price * product.ProductQuantity);

                var orderProduct = new OrderItem()
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductID,
                    Quantity = product.ProductQuantity,
                    Price = p.Price
                };

                await _appDbContext.OrderItem.AddAsync(orderProduct);
            }
            await _appDbContext.SaveChangesAsync();

            dto.ID = order.OrderId;

            Invoice invoice = new Invoice()
            {
                Total = total,
                Date = DateTime.UtcNow,
                OrderId = order.OrderId,    
            };
            await _appDbContext.AddAsync(invoice);
            await _appDbContext.SaveChangesAsync();

            return dto;
        }

        public async Task<OrderWithProductsDTO?> UpdateOrderAsync(OrderWithProductsDTO OrderWithProductsDto)
        {
            var order = await _appDbContext.orders.FindAsync(OrderWithProductsDto.ID);
            if (order == null) return null;
           
            order.Name = OrderWithProductsDto.Name!;
            order.OrderDate = OrderWithProductsDto.OrderDate;
            order.CustomerId = OrderWithProductsDto.CustomerID;
            await _appDbContext.SaveChangesAsync();

            var orderProducts =await _appDbContext.OrderItem.Where(op => op.OrderId == 2).ToListAsync();

            _appDbContext.OrderItem.RemoveRange(orderProducts);
            await _appDbContext.SaveChangesAsync();

            foreach (var product in OrderWithProductsDto.Products!)
            {
                var orderProduct = new OrderItem()
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductID,
                    Quantity = product.ProductQuantity
                };

                await _appDbContext.OrderItem.AddAsync(orderProduct);
            }
            await _appDbContext.SaveChangesAsync();

            return OrderWithProductsDto;
        }

        public async Task<Order?> DeleteOrderAsync(int id)
        {
            var order = await _appDbContext.orders.FindAsync(id);
            if (order == null) { return null; }

            _appDbContext.orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
            return order;
        }
    }
}
