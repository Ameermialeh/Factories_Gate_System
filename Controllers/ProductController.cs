using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductRepo _productRepo;
        public ProductController(ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }


        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var products = _productRepo.GetProducts();
            if (products == null)
            {
                return BadRequest("No Products here!!");
            }
            var productDto = products.Select(p => new ProductDTO()
            {
                ID = p.ProductId,
                Name = p.ProductName,
                Description = p.ProductDescription,
                Price = p.ProductPrice,
                Quantity = p.ProductQuantity,
            });
            return Ok(productDto);
        }

        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById(int id) {
            var product = _productRepo.GetProductById(id);
            if (product == null)
            {
                return BadRequest($"No product with id = {id}. Try again");
            }
            var productdto = new ProductDTO()
            {
                ID = product.ProductId,
                Name = product.ProductName,
                Description = product.ProductDescription,
                Price = product.ProductPrice,
            };
            return Ok(productdto);
        }

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] ProductDTO productdto)
        {
            var isAdded = _productRepo.CreateProduct(productdto);
            if (!isAdded) { return BadRequest("Somthing went wrong!"); }
            return Ok(productdto);
        }
    }
}
