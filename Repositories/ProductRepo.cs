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
                Quantity = p.StockQuantity,
                FactoryId = p.FactoryId,
            }).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _appDbContext.products.FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<ProductDTO?> CreateProductAsync(ProductDTO productdto)
        {
            try
            {
                Product product = new Product()
                {
                    Name = productdto.Name!,
                    Price = productdto.Price,
                    StockQuantity = productdto.Quantity,
                };

                await _appDbContext.products.AddAsync(product);
                await _appDbContext.SaveChangesAsync();

                productdto.ID = product.ProductId;
                return productdto;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProductDTO?> UpdateProductAsync(ProductDTO productdto)
        {
            var existingproduct = await _appDbContext.products.FindAsync(productdto.ID);
            if (existingproduct == null)
                return null;

            existingproduct.Name = productdto.Name!;
            existingproduct.Price = productdto.Price;
            existingproduct.StockQuantity = productdto.Quantity;
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
