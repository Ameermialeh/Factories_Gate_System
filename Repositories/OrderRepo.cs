using FactoriesGateSystem.DTOs.OrderDTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                Name = o.OrderName,
                Description = o.OrderDescription,
                OrderDate = o.OrderDate,
                CustomerID = o.CustomerId
            }).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id) {
            return await _appDbContext.orders.FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<List<OrderProductsDTO>> GetProductsForOrderAsync(int orderID)
        {
            var products =await _appDbContext.orderProducts.Where(op=> op.OrderId == orderID).Select(op => new OrderProductsDTO
            {
                ProductID = op.ProductId,
                ProductQuantity = op.Quantity
            }).ToListAsync();
            
            return products;
        }

        public async Task<bool> ChickIfAllProductsNotHideAsync(OrderWithProductsDTO dto)
        {
            var productIds = dto.Products!.Select(p => p.ProductID).ToList();

            return await _appDbContext.products.Where(p => productIds.Contains(p.ProductId)).AllAsync(p => !p.Hide);
        }

        public async Task<OrderWithProductsDTO> CreateOrderAsync(OrderWithProductsDTO dto)
        {
            Order order = new Order()
            {
                OrderName = dto.Name!,
                OrderDate = dto.OrderDate,
                OrderDescription = dto.Description!,
                CustomerId = dto.CustomerID
            };

            await _appDbContext.orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            foreach(var product in dto.Products!)
            {
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductID,
                    Quantity = product.ProductQuantity
                };

                await _appDbContext.orderProducts.AddAsync(orderProduct);
            }
            await _appDbContext.SaveChangesAsync();

            dto.ID = order.OrderId;
            return dto;
        }

        public async Task<OrderWithProductsDTO?> UpdateOrderAsync(OrderWithProductsDTO OrderWithProductsDto)
        {
            var order = await _appDbContext.orders.FindAsync(OrderWithProductsDto.ID);
            if (order == null) return null;
           
            order.OrderName = OrderWithProductsDto.Name!;
            order.OrderDate = OrderWithProductsDto.OrderDate;
            order.OrderDescription = OrderWithProductsDto.Description!;
            order.CustomerId = OrderWithProductsDto.CustomerID;
            await _appDbContext.SaveChangesAsync();

            var orderProducts =await _appDbContext.orderProducts.Where(op => op.OrderId == 2).ToListAsync();

            _appDbContext.orderProducts.RemoveRange(orderProducts);
            await _appDbContext.SaveChangesAsync();

            foreach (var product in OrderWithProductsDto.Products!)
            {
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductID,
                    Quantity = product.ProductQuantity
                };

                await _appDbContext.orderProducts.AddAsync(orderProduct);
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
