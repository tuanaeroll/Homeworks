using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using System.Linq;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 100, Description = "Description 1" },
            new Product { Id = 2, Name = "Product 2", Price = 200, Description = "Description 2" },
            new Product { Id = 3, Name = "Product 3", Price = 300, Description = "Description 3" }
        };

        // GET api/products (Model Binding with Query Parameters)
        [HttpGet]
        public IActionResult Get([FromQuery] string? name, [FromQuery] decimal? price)
        {
            var products = _products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (price.HasValue)
            {
                products = products.Where(p => p.Price == price.Value);
            }

            return Ok(products.ToList());
        }

        // GET api/products/list (Model Binding with Query Parameters & Listing with Sorting)
        [HttpGet("list")]
        public IActionResult GetList([FromQuery] string? name, [FromQuery] decimal? price, [FromQuery] string sortBy)
        {
            var products = _products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (price.HasValue)
            {
                products = products.Where(p => p.Price == price.Value);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "name")
                {
                    products = products.OrderBy(p => p.Name);
                }
                else if (sortBy.ToLower() == "price")
                {
                    products = products.OrderBy(p => p.Price);
                }
            }

            return Ok(products.ToList());
        }

        // POST api/products (Model Binding with Body)
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }

            _products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        // PUT api/products/{id} (Model Binding with Body)
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            return NoContent();
        }

        // DELETE api/products/{id} (Model Binding with Route)
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _products.Remove(product);
            return NoContent();
        }
    }
}
