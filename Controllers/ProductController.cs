using Microsoft.AspNetCore.Mvc;
using Sample.Context;
using Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace Sample.Controllers
{
    [Route("api/Products/[Controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductContext? _productDbContext;
        public ProductController(ProductContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_productDbContext == null)
            {
                return NotFound();
            }
            return await _productDbContext.Products.ToListAsync();

        }

        [HttpGet("id")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            if (_productDbContext == null)
            {
                return NotFound();
            }
            var ProductByID = await _productDbContext.Products.FindAsync(id);

            if (ProductByID is null)
            {
                return NotFound();
            }

            return ProductByID;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product is null)
            {
                return BadRequest();
            }
            if (_productDbContext is null)
            {
                return StatusCode(500);
            }
            _productDbContext.Products.Add(product);
            await _productDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Product), new { id = product.Id }, product);
        }

        [HttpPost("Products")]
        public async Task<ActionResult> PostProducts(List<Product> products)
        {
            if (products is null)
            {
                return StatusCode(400);
            }
            if (_productDbContext is null)
            {
                return StatusCode(500);
            }
            foreach (var product in products)
            {
                _productDbContext.Products.Add(product);
            }
            await _productDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("id")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            if (_productDbContext is null)
            {
                return StatusCode(500);
            }
            var product = await _productDbContext.Products.FindAsync(id);
            if (product is null)
            {
                return StatusCode(404);
            }
            _productDbContext.Products.Remove(product);

            return Ok();
        }
        [HttpDelete("Bulk")]
        public async Task<ActionResult> DeleteBulkById(List<int> ints)
        {

            if (_productDbContext is null)
            {
                return StatusCode(500);
            }
            foreach (var item in ints)
            {
                var product = await _productDbContext.Products.FindAsync(item);
                if (product is null)
                {
                    continue;
                }
                _productDbContext.Products.Remove(product);
            }
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> PutUpdateById(Product product)
        {
            if(product is null)
            {
                return StatusCode(400);
            }
            if(_productDbContext is null)
            {
                return StatusCode(500);
            }
            _productDbContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _productDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var oldproduct = await _productDbContext.Products.FindAsync(product);
                if (oldproduct is null)
                {
                    return NotFound();
                }
                return BadRequest(product);
            }
            return Ok();
        }

        [HttpPut("BulkUpdate")]
        public async Task<ActionResult> PutBulkUpdateById(List<Product> products)
        {
            if (products is null)
            {
                return StatusCode(400);
            }
            if (_productDbContext is null)
            {
                return StatusCode(500);
            }
            foreach (var product in products)
            {
                _productDbContext.Entry(product).State = EntityState.Modified;
            }
            try
            {
                await _productDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(products);
            }
            return Ok();
        }
    }
}
