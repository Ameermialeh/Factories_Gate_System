using FactoriesGateSystem.DTOs;
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
            var products = _appDbContext.products.ToList();
            if(func == null) { return products; }
            products = products.Where(func).ToList();
            return products;
        }

        public Product? GetProductById(int id)
        {
            var product = _appDbContext.products.FirstOrDefault(p => p.ProductId == id);
            return product;
        }

        public bool CreateProduct(ProductDTO productdto)
        {
            try
            {
                Product product = new Product()
                {
                    ProductId = productdto.ID,
                    ProductName = productdto.Name,
                    ProductDescription = productdto.Description,
                    ProductPrice = productdto.Price,
                    ProductQuantity = productdto.Quantity,
                };

                _appDbContext.products.Add(product);
                _appDbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
