using FactoriesGateSystem.DTOs.ProductDTOs;
using FactoriesGateSystem.Models;
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
            var product =  await _appDbContext.products.FirstOrDefaultAsync(p => p.ProductId == id);
            var inventory = await _appDbContext.inventories.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) { return null; }
            return new ProductDTO()
            {
                ID = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Quantity = inventory!.Quantity,
            };
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productdto, int factoryId)
        {

            Product product = new Product()
            {
                Name = productdto.Name!,
                Price = productdto.Price,
                FactoryId = factoryId
            };

            await _appDbContext.products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            productdto.ID = product.ProductId;

            Inventory inventory = new Inventory
            {
                MaterialId = 0,
                ProductId = product.ProductId,
                Quantity = productdto.Quantity,
                LastUpdated = DateTime.UtcNow.Date,
            };
            await _appDbContext.inventories.AddAsync(inventory);
            await _appDbContext.SaveChangesAsync();

            return productdto;
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO productdto)
        {
            var existingproduct = await _appDbContext.products.FindAsync(productdto.ID);
            if (existingproduct == null)
                return null;

            existingproduct.Name = productdto.Name!;
            existingproduct.Price = productdto.Price;
            //existingproduct.StockQuantity = productdto.Quantity;
            await _appDbContext.SaveChangesAsync();

            return productdto;
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            var product = await _appDbContext.products.FindAsync(id);
            if(product == null) { return null; }


            _appDbContext.products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return product;
    
        }
    }
}
