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
            IQueryable<Product> query = _appDbContext.products.Where(p => !p.Hide);
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(p => new ProductDTO()
            {
                ID = p.ProductId,
                Name = p.ProductName,
                Description = p.ProductDescription,
                Price = p.ProductPrice,
                Quantity = p.ProductQuantity,
            }).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _appDbContext.products.FirstOrDefaultAsync(p => p.ProductId == id && !p.Hide);
        }

        public async Task<ProductDTO?> CreateProductAsync(ProductDTO productdto)
        {
            try
            {
                Product product = new Product()
                {
                    ProductName = productdto.Name!,
                    ProductDescription = productdto.Description,
                    ProductPrice = productdto.Price,
                    ProductQuantity = productdto.Quantity,
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

            existingproduct.ProductName = productdto.Name!;
            existingproduct.ProductDescription = productdto.Description;
            existingproduct.ProductPrice = productdto.Price;
            existingproduct.ProductQuantity = productdto.Quantity;
            await _appDbContext.SaveChangesAsync();

            return productdto;
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            var product = await _appDbContext.products.FindAsync(id);
            if(product == null) { return null; }

            if(await _appDbContext.orderProducts.Where(op=> op.ProductId == product.ProductId).FirstOrDefaultAsync() == null)
            {
                _appDbContext.products.Remove(product);
                await _appDbContext.SaveChangesAsync();
                return product;
            }
            product.Hide = true;
            await _appDbContext.SaveChangesAsync();
            return product;
        }
    }
}
