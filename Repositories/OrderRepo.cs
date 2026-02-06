using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.OrderDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class OrderRepo : IOrderRepo
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
            }).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id) 
        {
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

        public async Task<OrderResponseDTO?> CreateOrderAsync(OrderWithProductsDTO dto, int factoryId)
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

            Invoice invoice = new Invoice()
            {
                Total = total,
                Date = dto.OrderDate,
                OrderId = order.OrderId,    
            };
            await _appDbContext.AddAsync(invoice);
            await _appDbContext.SaveChangesAsync();

            return new OrderResponseDTO { 
                ID = order.OrderId,
                Name = order.Name,
                OrderDate = order.OrderDate.Date,
                CustomerID = order.CustomerId,
                Products = dto.Products
            };
        }

        public async Task<OrderWithProductsDTO?> UpdateOrderAsync(int id, UpdateOrderDTO dto)
        {
            var order = await GetOrderByIdAsync(id);
            if (order == null) return null;
           
            if(dto.Name != null) 
            { 
                order.Name = dto.Name; 
            }
            if(dto.OrderDate != null)
            {
                order.OrderDate = dto.OrderDate.Value;
            }
            if (dto.CustomerID != null)
            {
                order.CustomerId = dto.CustomerID.Value;
            }

            await _appDbContext.SaveChangesAsync();

            List<OrderItemDTO> productsList = new();
            decimal total = 0;
            if (dto.Products != null)
            {
                var orderProducts = await _appDbContext.OrderItem.Where(op => op.OrderId == order.OrderId).ToListAsync();
                _appDbContext.OrderItem.RemoveRange(orderProducts);
                await _appDbContext.SaveChangesAsync();

                foreach (var product in dto.Products!)
                {
                    var orderProduct = new OrderItem()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.ProductID,
                        Quantity = product.ProductQuantity
                    };
                    await _appDbContext.OrderItem.AddAsync(orderProduct);
                    productsList.Add(new OrderItemDTO { ProductID = product.ProductID, ProductQuantity = product.ProductQuantity });

                    var p = await _appDbContext.products.FirstOrDefaultAsync(p => p.ProductId == product.ProductID);

                    if (p != null)
                        total += p.Price * product.ProductQuantity;
                }
                await _appDbContext.SaveChangesAsync();

                var inv = await _appDbContext.invoices.FirstOrDefaultAsync(i => i.OrderId == id);

                if (inv != null)
                {
                    inv.Total = total;
                    await _appDbContext.SaveChangesAsync();
                }
            }
            else
            {
                productsList = await _appDbContext.OrderItem
                    .Where(op => op.OrderId == order.OrderId)
                    .Select(op => new OrderItemDTO
                    {
                        ProductID = op.ProductId,
                        ProductQuantity = op.Quantity
                    })
                    .ToListAsync();
            }

            return new OrderWithProductsDTO
            {
                Name = order.Name,
                OrderDate = order.OrderDate,
                CustomerID = order.CustomerId,
                Products = productsList
            };
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _appDbContext.orders.FindAsync(id);
            if (order == null) { return false; }

            _appDbContext.orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
