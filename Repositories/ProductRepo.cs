using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class ProductRepo
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepo(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<List<ProductDTO>> GetProductsAsync(Expression<Func<Product,bool>>? filter = null)
        {
            IQueryable<Product> query = _appDbContext.products;
            if (filter != null)
                query = query.Where(filter);


            return await query.Select(p => new ProductDTO()
            {
                ID = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                //Quantity = p.StockQuantity,
            }).ToListAsync();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            return await _appDbContext.inventories
                .Where(i => i.ProductId == id)
                .Select(i => new ProductDTO
                {
                    ID = i.Product!.ProductId,
                    Name = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productdto, int factoryId)
        {

            Product product = new Product()
            {
                Name = productdto.Name!,
                Price = productdto.Price,
                FactoryId = factoryId
            };

            Inventory inventory = new Inventory
            { 
                Quantity = productdto.Quantity,
                LastUpdated = DateTime.UtcNow.Date,
                Product = product,
            };

            await _appDbContext.products.AddAsync(product);
            await _appDbContext.inventories.AddAsync(inventory);
            await _appDbContext.SaveChangesAsync();

            return new ProductDTO
            {
                ID = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Quantity = inventory.Quantity
            };
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO dto)
        {
            var inventory = await _appDbContext.inventories.Include(i => i.Product).FirstOrDefaultAsync(i => i.ProductId == dto.ID);

            if (inventory == null || inventory.Product == null)
                return null;

            inventory.Product.Name = dto.Name!;
            inventory.Product.Price = dto.Price;
            inventory.Quantity = dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            return new ProductDTO
            {
                ID = inventory.Product.ProductId,
                Name = inventory.Product.Name,
                Price = inventory.Product.Price,
                Quantity = inventory.Quantity
            };
        }

        public async Task<ProductDTO?> UpdateQuantityProductAsync(UpdateProductDTO dto)
        {
            var inventory = await _appDbContext.inventories.Include(i => i.Product).FirstOrDefaultAsync(i => i.ProductId == dto.ID);
            if (inventory == null || inventory.Product == null)
                return null;

            inventory.Quantity = dto.Quantity;
            inventory.LastUpdated = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();

            return new ProductDTO
            {
                ID = inventory.Product.ProductId,
                Name = inventory.Product.Name,
                Price = inventory.Product.Price,
                Quantity = inventory.Quantity
            };
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            var inventory = await _appDbContext.inventories.Include(i => i.Product).FirstOrDefaultAsync(i => i.ProductId == id);

            if (inventory == null || inventory.Product == null)
                return null;

            _appDbContext.inventories.Remove(inventory);
            _appDbContext.products.Remove(inventory.Product);

            await _appDbContext.SaveChangesAsync();

            return inventory.Product;
        }
    }
}
