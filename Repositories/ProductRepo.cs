using FactoriesGateSystem.DTOs.ProductDTOs;
using FactoriesGateSystem.Models;

namespace FactoriesGateSystem.Repositories
{
    public class ProductRepo
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepo(AppDbContext context)
        {
            _appDbContext = context;
        }

        public List<Product> GetProducts(Func<Product,bool>? func = null)
        {
            var products = _appDbContext.products.Where(p=>!p.Hide).ToList();
            if(func == null) { return products; }
            products = products.Where(func).ToList();
            return products;
        }

        public Product? GetProductById(int id)
        {
            var product = _appDbContext.products.FirstOrDefault(p => p.ProductId == id && !p.Hide);
            return product;
        }

        public ProductDTO? CreateProduct(ProductDTO productdto)
        {
            try
            {
                Product product = new Product()
                {
                    ProductName = productdto.Name,
                    ProductDescription = productdto.Description,
                    ProductPrice = productdto.Price,
                    ProductQuantity = productdto.Quantity,
                };

                _appDbContext.products.Add(product);
                _appDbContext.SaveChanges();

                productdto.ID = product.ProductId;
                return productdto;
            }
            catch
            {
                return null;
            }
        }

        public ProductDTO? UpdateProduct(ProductDTO productdto)
        {
            var existingproduct = GetProductById(productdto.ID);
            if (existingproduct == null)
                return null;

            existingproduct.ProductName = productdto.Name;
            existingproduct.ProductDescription = productdto.Description;
            existingproduct.ProductPrice = productdto.Price;
            existingproduct.ProductQuantity = productdto.Quantity;
            _appDbContext.SaveChanges();

            return productdto;
        }

        public Product? DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if(product == null) { return null; }

            if(_appDbContext.orderProducts.Where(op=> op.ProductId == product.ProductId).FirstOrDefault() == null)
            {
                _appDbContext.products.Remove(product);
                _appDbContext.SaveChanges();
                return product;
            }
            product.Hide = true;
            _appDbContext.SaveChanges();
            return product;
        }
    }
}
